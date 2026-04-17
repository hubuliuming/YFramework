---
name: yframework-autobind
description: "Use when the task is about YFramework AutoBind: `CONTEXT/MonoBehaviour/AutoBind`, `AutoBindEditor`, `AutoBindRules`, `IAutoBindMono`, `YMonoBehaviour` binding recursion, generated `*.Designer.cs`, automatic `partial` insertion, `[AutoBindField]`, `ParentMono`, node-name prefixes, field-name generation, array field generation, generated-reference backfill, or AutoBind test samples in `Assets/Tests/Scripts`."
---

# YFramework AutoBind

Use this skill for YFramework AutoBind work only. Keep it focused on the existing generator and binding chain.

## Source Of Truth

Read these first, using explicit UTF-8 for every `Doc/` file:

- `Doc/00-项目总览与模块地图.md`
- `Doc/01-核心运行时与框架基座.md`
- `Doc/02-AutoBind机制.md`
- `Doc/06-遗留模块与测试样例.md`

Treat undocumented behavior as `UNKNOWN`. If docs and code conflict, stop and report the conflict before changing anything.

## Core Files

- `Assets/YFramework/Framework/YMonoBehaviour.cs`
- `Assets/YFramework/Framework/AutoBindE/AutoBindRules.cs`
- `Assets/YFramework/Editor/AutoBindE/AutoBindEditor.cs`
- `Assets/YFramework/Kit/Type/TypeResolver.cs`
- `Assets/YFramework/Collections/SerializableKeyValue.cs`
- `Assets/YFramework/Extension/MonoBase/TransformExtension.cs`
- `Assets/Tests/Scripts/Test1.cs`
- `Assets/Tests/Scripts/Test1.Designer.cs`
- `Assets/Tests/Scripts/Test2.cs`
- `Assets/Tests/Scripts/Test2.Designer.cs`
- `Assets/Tests/Scripts/Test3.cs`

## Workflow

1. Decide whether the change touches recognition rules, generated code shape, recursive scan boundaries, or field backfill.
2. Trace the entry from `CONTEXT/MonoBehaviour/AutoBind` through generated `*.Designer.cs` and `[DidReloadScripts]` backfill.
3. Preserve existing node prefix behavior unless the confirmed plan explicitly changes `AutoBindRules`.
4. Preserve generated-stage `Transform` backfill as the priority source for single fields and array fields.
5. Treat child `IAutoBindMono` / `YMonoBehaviour` nodes as separate binding boundaries.
6. Compare any generator change against the `Assets/Tests/Scripts` samples.
7. Update only the affected `Doc/` sections after real changes.

## Rules

- Do not manually maintain generated `*.Designer.cs` as business logic.
- Do not change Prefab, Scene, Animator Controller, or hierarchy structure from this skill.
- Do not make AutoBind a general UI framework or message system.
- Do not assume `Assets/Old` is a current implementation path.
- Do not rely on terminal-garbled Chinese documentation.
- If changing generator output, check whether existing generated samples still represent the intended shape.
- If adding or changing a prefix, update both the code rule and the relevant documentation.
- If a task needs resource hierarchy changes, return the required manual resource adjustment instead of editing the resource.

## Verification

Prefer focused verification:

- Inspect `git diff -- Assets/YFramework/Editor/AutoBindE/AutoBindEditor.cs Assets/YFramework/Framework/AutoBindE/AutoBindRules.cs Doc/02-AutoBind机制.md`.
- Build editor/runtime projects when code changed: `dotnet build YFramework.Editor.csproj --no-restore` and/or `dotnet build YFramework.csproj --no-restore`.
- If Unity is available, use editor-side AutoBind samples to confirm generated fields and backfilled references.
