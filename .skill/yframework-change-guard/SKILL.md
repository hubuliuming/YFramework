---
name: yframework-change-guard
description: "Use for any YFramework project code change, documentation synchronization, final verification, or implementation handoff that must obey `AGENTS.md`: read `Doc/` first, require a confirmed plan before implementation, keep changes minimal, avoid Animator/Prefab/Scene structure edits, update `FACT` / `CURRENT STRATEGY` / `KNOWN ISSUES` / `CHANGE LOG`, read and write Chinese docs as UTF-8, inspect `git diff`, and verify affected Unity C# projects."
---

# YFramework Change Guard

Use this skill as the project-level guardrail for YFramework changes. It does not replace task-specific skills such as `$yframework-autobind`; use both when relevant.

## Source Of Truth

Read the relevant `Doc/` files first with explicit UTF-8:

- `Doc/00-项目总览与模块地图.md`
- `Doc/01-核心运行时与框架基座.md`
- `Doc/02-AutoBind机制.md`
- `Doc/03-通用能力库.md`
- `Doc/04-UI系统.md`
- `Doc/05-编辑器工具.md`
- `Doc/06-遗留模块与测试样例.md`

Treat `Doc/` as the project source of truth. Treat undocumented behavior as `UNKNOWN`.

## Required Flow

1. Read the affected docs and current code entry points.
2. Identify the real control entry, call chain, affected files, risk points, and verification method.
3. If there is no confirmed plan, output a plan or adjustment plan and wait for confirmation.
4. After implementation, update only the affected documentation sections with facts that actually changed.
5. Inspect `git diff -- <file>` for every changed Chinese document and confirm new Chinese text is not garbled.
6. Verify the smallest relevant build or Unity check available.
7. Report remaining manual GamePlayer / Unity Editor checks separately from automated verification.

## Hard Stops

Stop and report a blocker if any of these occur:

- Docs and code conflict.
- The control entry or call chain cannot be found.
- The confirmed plan cannot land inside the current structure.
- The task requires new resource hierarchy, Scene objects, Prefab hierarchy, or Animator Controller structure changes.
- The task requires widening scope beyond the confirmed plan.
- Chinese doc output appears garbled.

## Project Boundaries

- Default mainline is `Assets/YFramework`.
- `Assets/Old` is historical reference unless the confirmed task explicitly targets it.
- `Assets/Tests` is sample or validation code, especially for AutoBind.
- `Msg.cs`, `TimerKit.cs`, and `ActionSpan.cs` are not complete systems.
- UI is a lightweight display stack plus helpers, not a full page-routing framework.
- Editor code must keep `UnityEditor` dependencies out of runtime assemblies.

## Documentation Update Rules

Update only affected sections:

- `FACT`: actual parameter, call relationship, control logic, or landed behavior changes.
- `CURRENT STRATEGY`: actual behavior strategy changes.
- `KNOWN ISSUES`: resolved issues removed or marked, new real issues added.
- `CHANGE LOG`: append modification content, reason, impact scope, structural involvement, and manual action need.

Do not add suggestions, future plans, or assumptions as completed facts.

## Verification Commands

Pick only what matches the change:

- Runtime: `dotnet build YFramework.csproj --no-restore`
- Editor: `dotnet build YFramework.Editor.csproj --no-restore`
- UI editor: `dotnet build YFramework.UI.Editor.csproj --no-restore`

If a command cannot run because Unity-generated project files or local environment are stale, state that explicitly.
