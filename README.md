# YFramework

YFramework 是一套面向 Unity 的轻量工具型基础库，当前主干代码位于 `Assets/YFramework`。

它更偏工具库，不是完整业务框架。核心能力包括：

- `AutoBind`：根据节点命名规则自动生成字段绑定。
- `Framework`：统一运行时基类与全局入口。
- `Kit` / `Extension`：延迟、计时器、单例、常用工具与扩展方法。
- `Network`：HTTP、Protobuf 与旧 TCP/UDP 封装。
- `Components` / `Interaction`：可挂载组件与非 UI 指针事件辅助。
- `UI`：轻量 UI 栈、自定义 Image、布局与事件辅助。
- `Editor`：AutoBind、自动保存、资源导入、UI 菜单与 YFramework MCP 工具。

## 目录说明

| 路径 | 说明 |
| --- | --- |
| `Assets/YFramework` | 当前主干框架代码 |
| `Assets/Tests` | 示例与验证脚本 |
| `Doc` | 面向 AI 和维护者的模块化文档 |
| `Assets/Old` | 当前仓库中已移除，仅在文档中保留历史说明 |

## 阅读顺序

1. [Doc/00-项目总览与模块地图.md](Doc/00-项目总览与模块地图.md)
2. [Doc/01-核心运行时与框架基座.md](Doc/01-核心运行时与框架基座.md)
3. [Doc/02-AutoBind机制.md](Doc/02-AutoBind机制.md)
4. [Doc/03-通用能力库.md](Doc/03-通用能力库.md)
5. [Doc/04-UI系统.md](Doc/04-UI系统.md)
6. [Doc/05-编辑器工具.md](Doc/05-编辑器工具.md)
7. [Doc/06-遗留模块与测试样例.md](Doc/06-遗留模块与测试样例.md)

## AutoBind 快速使用

1. 给目标 `GameObject` 挂上脚本。
2. 按命名规则给子节点命名。
3. 在 Inspector 中选中组件，打开组件右上角菜单，执行 `AutoBind`。
4. 框架会补 `partial class`、生成 `Xxx.Designer.cs`，并在脚本重载后回填字段。

常见前缀：

| 前缀 | 类型 |
| --- | --- |
| `Go` | `GameObject` |
| `Rect` | `RectTransform` |
| `Btn` | `Button` |
| `Txt` | `Text` |
| `Img` | `Image` |
| `Tog` | `Toggle` |
| `Sld` | `Slider` |
| `ScoV` | `ScrollRect` |
| `RawImg` | `RawImage` |
| `Anim` | `Animator` |
| `Rig` | `Rigidbody` |
| `Rig2` | `Rigidbody2D` |
| `Col` | `Collider` |
| `Col2` | `Collider2D` |

常见命名示例：

- `BtnClose`
- `TxtTitle`
- `ImgIcon`
- `RectContent`
- `TogMusic`

关键文件：

- [Assets/YFramework/Framework/AutoBindE/AutoBindRules.cs](Assets/YFramework/Framework/AutoBindE/AutoBindRules.cs)
- [Assets/YFramework/Editor/AutoBindE/AutoBindEditor.cs](Assets/YFramework/Editor/AutoBindE/AutoBindEditor.cs)
- [Assets/Tests/Scripts/Test1.cs](Assets/Tests/Scripts/Test1.cs)
- [Assets/Tests/Scripts/Test1.Designer.cs](Assets/Tests/Scripts/Test1.Designer.cs)
- [Assets/Tests/Scripts/Test2.cs](Assets/Tests/Scripts/Test2.cs)
- [Assets/Tests/Scripts/Test2.Designer.cs](Assets/Tests/Scripts/Test2.Designer.cs)

## 模块速览

| 模块 | 路径 | 重点入口 |
| --- | --- | --- |
| Framework | `Assets/YFramework/Framework` | `YMonoBehaviour`、`MonoGlobal`、`AutoBindRules` |
| AutoBind Editor | `Assets/YFramework/Editor/AutoBindE` | `AutoBindEditor` |
| Kit / Extension | `Assets/YFramework/Kit`、`Assets/YFramework/Extension` | `ActionKit`、`TimerManager`、`MonoSingleton`、`TransformExtension` |
| Network | `Assets/YFramework/Network` | `HttpService`、`ProtoSerializer`、`TcpClient` |
| Components / Interaction | `Assets/YFramework/Components`、`Assets/YFramework/Interaction` | `RotationCam`、`FPCharacter`、`Trigger2DCheck`、`BaseButton` |
| Collections / Math | `Assets/YFramework/Collections`、`Assets/YFramework/Math` | `SerializableDictionary`、`SerializableKeyValue`、`Fixed64` |
| UI | `Assets/YFramework/UI` | `GameUIKit`、`UIKitRuntime`、`CircleImage`、`SlideScrollHorizontal` |
| Editor | `Assets/YFramework/Editor`、`Assets/YFramework/UI/Editor` | `AutoSaveWindow`、`InputResourcesSetting`、`YFrameworkMcpTools` |

## UnityMCP / Codex

项目已接入 `com.coplaydev.unity-mcp`，本机 Codex 推荐使用 HTTP 方式连接：

```bash
codex mcp add unityMCP --url http://localhost:8080/mcp
```

当前 YFramework 自定义 MCP 工具：

- `yf_autosave_config`
- `yf_get_framework_info`
- `yf_autobind_generate`

## 维护约定

- 默认把 `Assets/YFramework` 当成当前主干实现。
- `Assets/Tests` 只作为示例和验证参考。
- `Assets/Old` 当前已移除，不应继续扩展新功能。
- 源码头注释存在中文编码问题时，优先看目录、类型名、方法名和真实调用关系。
- MCP 连接默认优先使用 streamable HTTP / `--url`，仅在 HTTP 不可用或明确要求时回退 stdio。

## 注意

当前仓库里有少量占位或未完成入口，不要默认当成成型系统：

- `Assets/YFramework/Msg/Msg.cs`
- `Assets/YFramework/Kit/Scheduling/TimerKit.cs`
- `Assets/YFramework/Kit/Scheduling/ActionSpan.cs`
