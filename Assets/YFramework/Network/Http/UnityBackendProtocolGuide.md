# Unity 前端后端协议格式说明

本文从当前 Unity 项目的前端请求实现中抽出通用协议格式，去掉具体项目业务说明，只保留其他 Unity 项目可复用的接入口径与关键示例。

## 1. 总体形态

客户端通过 `UnityWebRequest` 访问后端。

常规业务接口采用：

- 传输层：HTTP
- 请求方法：`POST`
- Body：protobuf 二进制
- Content-Type：`application/x-protobuf`
- 鉴权：登录接口不带 token；登录成功后的业务接口由业务 Service 层写入 `token` Header
- endpoint：默认使用协议命令号 `cmd` 作为 URL path，例如 `1002`、`2100`

配置表拉取属于独立格式，使用 JSON 包裹数据，不和 protobuf 业务接口混写。

## 2. 分层建议

建议把网络代码分成三层：

| 层级 | 职责 |
| --- | --- |
| HTTP 传输层 | 只负责 URL 拼接、GET / POST、Header、Timeout、原始字节收发 |
| protobuf 编解码层 | 只负责消息对象和 protobuf byte[] 的互转，以及可替换的 packet 编解码 |
| 业务 Service 层 | 负责选择 cmd、构造请求对象、添加 token、校验返回、把结果写入运行时模型 |

这种分层的核心原则是：通用传输层不理解业务，token 也不放到全局 HTTP 层，而是由需要鉴权的业务 Service 自己添加。

## 3. 业务接口请求格式

### 3.1 URL

默认 URL 由环境 `BaseUrl` 和 endpoint path 拼接：

```text
{BaseUrl}/{cmd}
```

示例：

```text
http://example.com/1002
http://example.com/2100
```

如果调用方传入绝对 URL，则直接使用该 URL。

### 3.2 Header

登录接口：

```text
Content-Type: application/x-protobuf
```

登录后的业务接口：

```text
Content-Type: application/x-protobuf
token: <login-token>
```

可按调试需要增加业务 Trace Header，例如生产类操作可增加：

```text
X-Trace-Id: <trace-id>
```

### 3.3 Body

Body 是请求消息对象序列化后的 protobuf 二进制。

当前通用 packet 编码保留了 `cmd + body` 的替换入口，但默认实现直接发送 body：

```text
requestBody = Serialize(requestMessage)
requestPacket = EncodePacket(cmd, requestBody)
POST requestPacket
```

当前默认：

```text
EncodePacket(cmd, body) = body
```

因此其他项目接入时需要和后端确认：HTTP path 上的 `cmd` 是否已经足够，还是还需要在 body 外层再包一层 packet。

### 3.4 Response

返回体同样按 protobuf 二进制解析：

```text
responsePacket = DecodePacket(rawBytes)
responsePayload = responsePacket.Body 非空 ? responsePacket.Body : rawBytes
response = Deserialize<ResponseType>(responsePayload)
```

当前默认：

```text
DecodePacket(rawBytes).Body = rawBytes
```

如果后端返回错误 protobuf，可通过响应 Header 标识消息类型，例如：

```text
Content-Type: application/x-protobuf
X-Protobuf-Message: proto.client.ErrorMsg
```

客户端再按错误消息结构解析错误码、错误文案和对应 cmd。

## 4. cmd 示例

| cmd | 示例含义 | 请求 |
| --- | --- | --- |
| `1002` | 登录 | `LoginReq` |
| `2100` | 关卡完成 | `StageCompleteReq` |
| `2001` | 通用业务操作 A | `BusinessActionAReq` |
| `2002` | 通用业务操作 B | `BusinessActionBReq` |
| `2003` | 通用业务操作 C | `BusinessActionCReq` |
| `2004` | 通用业务操作 D | `BusinessActionDReq` |

这些名称只是示例命名。其他 Unity 项目可以保留同样的 `cmd -> Request / Response` 映射方式，但应替换成自己的业务协议表。

## 5. 关键示例

### 5.1 登录

登录请求不带 token。

```text
POST /1002
Content-Type: application/x-protobuf
Body: LoginReq protobuf bytes
```

`LoginReq` 的关键字段示例：

| 字段号 | 字段 | 类型 | 说明 |
| --- | --- | --- | --- |
| `1` | `account` | string | 账号 |
| `2` | `password` | string | 密码或登录凭据 |
| `3` | `channelId` | int32 | 渠道 ID |
| `5` | `machineId` | string | 设备标识 |
| `9` | `platform` | string | 平台标识 |
| `13` | `regSource` | enum / int32 | 注册或登录来源 |

`LoginResp` 通常返回：

| 字段号 | 字段 | 类型 | 说明 |
| --- | --- | --- | --- |
| `1` | `token` | string | 后续业务接口鉴权 token |
| `2` | `userId` | int32 | 用户 ID |
| `11` | `gold` | int64 | 资源绝对值示例 |
| `12` | `energy` | int32 | 资源绝对值示例 |
| `13` | `goldLastUpdateTime` | int64 | 服务端资源时间戳示例 |
| `14` | `energyLastUpdateTime` | int64 | 服务端资源时间戳示例 |
| `15` | `attribute` | repeated int32 | 属性等级列表示例 |
| `16` | `stageId` | int32 | 进度值示例 |
| `17` | `item` | repeated message | 道具数量列表示例 |

登录成功后，客户端把 `token` 保存到会话模型，后续业务请求从会话模型读取 token 并写入 Header。

### 5.2 关卡完成

关卡完成是一个典型的登录后业务接口。

```text
POST /2100
Content-Type: application/x-protobuf
token: <login-token>
Body: StageCompleteReq protobuf bytes
```

`StageCompleteReq` 示例：

| 字段号 | 字段 | 类型 | 说明 |
| --- | --- | --- | --- |
| `1` | `id` | int32 | 当前关卡或任务 ID |
| `2` | `type` | int32 | 完成类型，例如 `0` 或 `1` |

`StageCompleteResp` 示例：

| 字段号 | 字段 | 类型 | 说明 |
| --- | --- | --- | --- |
| `1` | `id` | int32 | 服务端确认的 ID |
| `2` | `treasureUpdate` | message | 资源变化结果 |

客户端应以服务端返回为准更新本地会话数据，不要只依赖客户端本地配置推算奖励。

### 5.3 通用资源回写

多个业务接口可以复用同一个资源变化结构。

`TreasureUpdate` 示例：

| 字段号 | 字段 | 类型 | 说明 |
| --- | --- | --- | --- |
| `1` | `gold` | optional int64 | 金币等长整型资源的最新绝对值 |
| `2` | `energy` | optional int32 | 体力等整型资源的最新绝对值 |
| `3` | `item` | repeated message | 道具数量列表 |

`Item` 示例：

| 字段号 | 字段 | 类型 | 说明 |
| --- | --- | --- | --- |
| `1` | `id` | int32 | 道具 ID |
| `2` | `count` | int64 | 道具最新数量 |

推荐语义：

- 返回了某个字段，就按服务端绝对值覆盖本地缓存。
- 没返回的字段，保持本地原值。
- 列表型字段表示服务端确认后的最新数量，而不是客户端增量。

## 6. 配置表拉取格式

配置表拉取不是 protobuf 业务接口。它是单独的 JSON 文本格式。

请求形态示例：

```text
POST /loadConf?key=<ConfigKey>
Body: empty
```

返回包裹示例：

```json
{
  "key": "Level",
  "desc": "配置说明",
  "content": "[{\"Id\":1}]"
}
```

其中：

| 字段 | 说明 |
| --- | --- |
| `key` | 配置表名 |
| `desc` | 配置描述 |
| `content` | JSON 字符串，内部再反序列化为对应配置表列表 |

注意：`content` 是字符串，不是直接内嵌数组。客户端需要先解析外层 envelope，再按配置类型解析 `content`。

## 7. Unity 接入要点

1. 环境层只保存环境名、BaseUrl、超时和证书策略。
2. HTTP 层只返回原始状态、Header、RawBytes、错误信息和耗时。
3. protobuf 层统一注册每个请求 / 响应类型的 encoder、decoder。
4. Service 层统一负责 `cmd`、endpoint、token、请求校验和响应校验。
5. 运行时模型只消费 Service 的成功结果，不直接关心 HTTP 或 protobuf 细节。
6. packet 外层是否包含 `cmd` 必须和后端确认，并通过统一 packet codec 替换，不要散落在各个业务接口里。

按这个方式拆分后，其他 Unity 项目只需要替换协议消息、cmd 常量和业务 Service，HTTP 与 protobuf 基础设施可以保持稳定。
