# YFramework

YFramework 是一套面向 Unity 的轻量工具型基础库，当前主干代码位于 `Assets/YFramework`。

它的核心特点不是“大而全的业务框架”，而是提供一组可复用的基础能力：

- `AutoBind`：根据节点命名规则自动生成字段绑定。
- `Framework`：统一的运行时基类与全局入口。
- `Kit` / `Extension`：延迟、计时器、单例、网络、常用扩展方法等工具集合。
- `UI`：轻量 UI 基类、简单 UI 栈、自定义 Image 与布局辅助。
- `Editor`：AutoBind 生成器、自动保存、资源导入规则、UI 快捷创建工具。

## 当前目录说明

- `Assets/YFramework`
  - 当前主干框架代码。
- `Assets/Old`
  - 遗留模块，仅建议作为历史实现参考。
- `Assets/Tests`
  - 示例与验证脚本，可用于理解 AutoBind 和部分功能接入方式。
- `Doc`
  - 根据当前项目结构整理的模块化文档，适合 AI 和维护者快速建立认知。

## 快速理解项目

如果你第一次接触这个仓库，建议优先阅读：

1. [Doc/00-项目总览与模块地图.md](Doc/00-项目总览与模块地图.md)
2. [Doc/01-核心运行时与框架基座.md](Doc/01-核心运行时与框架基座.md)
3. [Doc/02-AutoBind机制.md](Doc/02-AutoBind机制.md)
4. [Doc/03-通用能力库.md](Doc/03-通用能力库.md)
5. [Doc/04-UI系统.md](Doc/04-UI系统.md)
6. [Doc/05-编辑器工具.md](Doc/05-编辑器工具.md)
7. [Doc/06-遗留模块与测试样例.md](Doc/06-遗留模块与测试样例.md)

## AutoBind 快速使用

AutoBind 是当前项目最值得先掌握的能力之一。

### 使用步骤

1. 给目标 `GameObject` 挂上脚本。
2. 按命名规则给子节点命名。
3. 在 Inspector 中选中该组件，打开组件右上角菜单，执行 `AutoBind`。
4. 框架会自动：
   - 将类补成 `partial class`（如果原来不是）。
   - 在同目录生成 `Xxx.Designer.cs`。
   - 在脚本重载后把匹配到的对象回填到字段。

### 常见命名前缀

以下前缀会被 AutoBind 识别：

- `Go`：`GameObject`
- `Rect`：`RectTransform`
- `Btn`：`Button`
- `Txt`：`Text`
- `Img`：`Image`
- `Tog`：`Toggle`
- `Sld`：`Slider`
- `ScoV`：`ScrollRect`
- `RawImg`：`RawImage`
- `Anim`：`Animator`
- `Rig`：`Rigidbody`
- `Rig2`：`Rigidbody2D`
- `Col`：`Collider`
- `Col2`：`Collider2D`

例如：

- `BtnClose`
- `TxtTitle`
- `ImgIcon`
- `RectContent`
- `TogMusic`

完整规则定义见：

- [Assets/YFramework/Framework/AutoBindE/AutoBindRules.cs](Assets/YFramework/Framework/AutoBindE/AutoBindRules.cs)

生成与回填逻辑见：

- [Assets/YFramework/Editor/AutoBindE/AutoBindEditor.cs](Assets/YFramework/Editor/AutoBindE/AutoBindEditor.cs)

示例脚本见：

- [Assets/Tests/Scripts/Test1.cs](Assets/Tests/Scripts/Test1.cs)
- [Assets/Tests/Scripts/Test1.Designer.cs](Assets/Tests/Scripts/Test1.Designer.cs)
- [Assets/Tests/Scripts/Test2.cs](Assets/Tests/Scripts/Test2.cs)
- [Assets/Tests/Scripts/Test2.Designer.cs](Assets/Tests/Scripts/Test2.Designer.cs)

## 核心模块概览

### Framework

核心入口位于：

- [Assets/YFramework/Framework/YMonoBehaviour.cs](Assets/YFramework/Framework/YMonoBehaviour.cs)
- [Assets/YFramework/Framework/MonoGlobal.cs](Assets/YFramework/Framework/MonoGlobal.cs)

说明：

- `YMonoBehaviour` 是项目统一基类，也是 AutoBind 生态里的重要接口入口。
- `MonoGlobal` 是全局常驻 `MonoBehaviour`，常被用作静态工具协程宿主。

### Kit / Extension

主要位于：

- `Assets/YFramework/Kit`
- `Assets/YFramework/Extension`

包含：

- `ActionKit`
- `TimerManager`
- `MonoSingleton`
- `NormalSingleton`
- `TcpClient` / `TcpServer` / `UDPClient`
- `TransformExtension` 等常用扩展方法

### UI

主要位于：

- `Assets/YFramework/UI`

包含：

- `UIBase` / `UIManager`
- `CircleImage`
- `PolygonColliderImage`
- `SlideScrollHorizontal`
- `RawImageAdaptivityLayout`

### Editor

主要位于：

- `Assets/YFramework/Editor`
- `Assets/YFramework/UI/Editor`

包含：

- AutoBind 生成器
- 场景自动保存
- 资源导入规则
- 自定义 UI 创建菜单与 Inspector

## 维护约定

- 默认把 `Assets/YFramework` 当成当前主干实现。
- `Assets/Old` 不应默认继续扩展新功能。
- `Assets/Tests` 更适合作为示例和验证参考，不应直接等同于正式业务代码。
- 项目中部分源码头注释存在中文编码问题，阅读时应优先以目录结构、类名、方法名和实际调用关系为准。

## 备注

当前仓库里有少量占位或未完成入口，例如：

- `Assets/YFramework/Msg/Msg.cs`
- `Assets/YFramework/Kit/Time/TimerKit.cs`
- `Assets/YFramework/Kit/Action/ActionSpan.cs`

阅读和扩展时，不要默认把这些文件当成已经成型的系统入口。
