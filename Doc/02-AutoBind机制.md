# AutoBind 机制

## 范围

AutoBind 是 YFramework 最核心、最有项目识别度的能力之一。
相关主文件分布在三块：

- 规则定义：`Assets/YFramework/Framework/AutoBindE/AutoBindRules.cs`
- 编辑器生成与回填：`Assets/YFramework/Editor/AutoBindE/AutoBindEditor.cs`
- 样例：`Assets/Tests/Scripts/*.cs` 与对应 `*.Designer.cs`

## 它解决什么问题

AutoBind 用来把层级里的子节点引用，自动生成到脚本字段里。
典型目标是减少手工拖 Inspector 引用的工作量。

最终效果通常是：

- 业务脚本变成 `partial class`
- 自动生成同名 `*.Designer.cs`
- 生成带 `[AutoBindField]` 的字段
- 脚本重载后，把层级对象自动回填到这些字段里

## 整体流程

### 1. 选择一个目标 MonoBehaviour

在 Inspector 的组件上下文菜单里执行：

- `CONTEXT/MonoBehaviour/AutoBind`

这个入口定义在 `AutoBindEditor` 中。

### 2. 自动补 `partial`

如果源脚本里没有 `partial class Xxx`，编辑器会直接改写源代码，把原类声明改成 `partial class`。

### 3. 生成 `Xxx.Designer.cs`

生成文件与原脚本放在同目录，内容包括：

- `[AutoBindField]` 字段
- 自动推导出的字段名
- 子 AutoBind Mono 的 `ParentMono`

### 4. 脚本重载后执行真正绑定

`[DidReloadScripts]` 回调触发 `BindAfterReload()`，然后根据缓存的扫描结果，把场景树里的对象回填到字段上。

## 命名规则来源

AutoBind 是否识别一个节点，核心取决于**节点名的前缀**。
这些前缀在 `AutoBindRules.BindElementTypes` 中定义。

当前主要映射如下：

| 前缀 | 绑定类型 |
| --- | --- |
| `Go` | `GameObject` |
| `Rect` | `RectTransform` |
| `Anim` | `Animator` |
| `Rig` | `Rigidbody` |
| `Rig2` | `Rigidbody2D` |
| `Col` | `Collider` |
| `Col2` | `Collider2D` |
| `Img` | `UnityEngine.UI.Image` |
| `Txt` | `UnityEngine.UI.Text` |
| `TMP` | `TMPro.TextMeshProUGUI`，可选 |
| `TMP_InputField` | `TMPro.TMP_InputField`，可选 |
| `TMP_Dropdown` | `TMPro.TMP_Dropdown`，可选 |
| `RawImg` | `UnityEngine.UI.RawImage` |
| `Tog` | `UnityEngine.UI.Toggle` |
| `Sld` | `UnityEngine.UI.Slider` |
| `ScoB` | `UnityEngine.UI.Scrollbar` |
| `ScoV` | `UnityEngine.UI.ScrollRect` |
| `Btn` | `UnityEngine.UI.Button` |
| `Drod` | `UnityEngine.UI.Dropdown` |
| `Ipf` | `UnityEngine.UI.InputField` |
| `Cvas` | `UnityEngine.Canvas` |

说明：

- `TMP*` 相关类型是“可选绑定元素”。
- 只有当项目里真的能解析到对应 TMPro 类型时，才会加入规则表。

## 字段名生成规则

字段名并不是直接使用节点原名，而是经过了一次规范化处理。
`AutoBindEditor.GetProcessMemberName()` 的核心规则是：

- 去掉非字母、数字、下划线字符
- 去掉末尾下划线
- 如果首字符是数字，把这个数字移到末尾
- 默认去掉末尾连续数字

所以在实际使用上，应该优先遵守下面的命名习惯：

- `BtnClose`
- `TxtTitle`
- `ImgIcon`
- `GoRoot`

尽量不要依赖复杂字符、编号和特殊分隔符。

## 数组字段生成规则

当多个同类型节点在规范化后得到相同成员名时，AutoBind 会把它们合并成数组字段。

例如：

- `BtnItem1`
- `BtnItem2`
- `BtnItem3`

最终可能生成：

- `public UnityEngine.UI.Button[] BtnItems;`

注意点：

- 数组字段名是在单数成员名后追加 `s`
- 旧的单字段声明会被替换成数组声明
- 数组项回填时依赖节点搜索与去重逻辑

## 递归绑定与子 Mono 处理

AutoBind 不只是绑定普通控件。
当子节点上挂载了 `IAutoBindMono` 或 `YMonoBehaviour` 子类时，还会继续递归处理。

这会带来两类效果：

### 1. 子组件会单独生成自己的 Designer

父节点扫描到子 `IAutoBindMono` 后，会继续对该子组件执行 AutoBind。

### 2. 子组件会得到 `ParentMono`

如果这个子组件是从父组件递归进去的，生成的 Designer 文件里会额外增加：

- `public ParentType ParentMono;`

`Assets/Tests/Scripts/Test2.Designer.cs` 就是典型例子。

## 实际样例

测试目录里已经提供了一套可直接参考的最小样本：

- `Assets/Tests/Scripts/Test1.cs`
- `Assets/Tests/Scripts/Test1.Designer.cs`
- `Assets/Tests/Scripts/Test2.cs`
- `Assets/Tests/Scripts/Test2.Designer.cs`

从这些文件可以直接看出：

- 原始类如何写
- Designer 如何生成
- 父子绑定如何建立
- 数组字段长什么样

## 回填依赖的搜索方式

字段回填时主要依赖：

- `transform.FindRecursive(...)`
- `transform.FindsRecursive(...)`

也就是说，**层级名称是否唯一、查找结果是否歧义**，会直接影响绑定可靠性。

如果多个节点名字重复，AutoBind 会尝试用 `processedTrans` 跳过已经用过的对象，但这依然不是强一致保证。

## 失败与风险点

### 1. 会改源脚本

第一次执行 AutoBind 时，源脚本可能被直接插入 `partial` 关键字。
这不是只读生成器，而是会修改业务代码。

### 2. 生成文件会重写

`*.Designer.cs` 是生成物，不应手工长期维护。
如果要自定义逻辑，应写在原始 `.cs` 文件里。

### 3. 节点重名会让结果不稳定

特别是在数组和递归绑定场景中，重名节点会增加出错概率。

### 4. 类型必须真实存在

如果前缀推导出的目标组件类型在目标节点上不存在，回填时会报错。

### 5. `IgnoreSelf` 会影响递归结果

实现了 `IAutoBindMono` 的子节点如果返回 `IgnoreSelf = true`，父级递归会跳过该节点。

## 推荐命名实践

为了让 AutoBind 稳定，建议统一使用下面的层级命名风格：

- `BtnClose`
- `BtnItem1`
- `BtnItem2`
- `TxtTitle`
- `ImgIcon`
- `TogMusic`
- `RectContent`

不建议：

- 带空格
- 带中文标点
- 同级重复名过多
- 过度依赖编号推导

## AI 修改建议

如果你要改 AutoBind，请优先判断：

- 是要改“识别规则”还是改“生成逻辑”？
- 是否会破坏已有 `*.Designer.cs` 生成结果？
- 是否会影响测试样例目录里的现有字段形态？
- 是否需要同步更新 `AutoBindRules` 与 `AutoBindEditor` 两端？

经验上，AutoBind 的稳定性依赖于“三件事必须一致”：

- 命名前缀规则
- 字段名推导规则
- 回填搜索规则

这三处只改其中一处，最容易留下隐性 bug。
