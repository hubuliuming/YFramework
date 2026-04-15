# Codex Profile Switcher

`CodexProfileSwitcher.ps1` snapshots login-related files from your local Codex home and restores them later.

It is intentionally designed to keep local thread history shared by leaving these paths untouched:

- `sessions`
- `archived_sessions`
- `session_index.jsonl`

## What it tracks

- `auth.json`
- `config.toml`
- `.codex-global-state.json`
- `cap_sid`
- `state_*.sqlite*`

## Typical workflow

1. Close Codex Desktop.
2. Log in with ChatGPT, confirm the app works, then save that state:

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\CodexProfileSwitcher.ps1 save -Profile chatgpt
```

3. Switch Codex to API key mode, confirm it works, then save that state:

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\CodexProfileSwitcher.ps1 save -Profile apikey
```

4. Later, when you want to switch back, close Codex Desktop and activate the profile you want:

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\CodexProfileSwitcher.ps1 activate -Profile chatgpt
```

or

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\CodexProfileSwitcher.ps1 activate -Profile apikey
```

## Helpful commands

Check current local status:

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\CodexProfileSwitcher.ps1 status
```

List saved profiles:

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\CodexProfileSwitcher.ps1 list
```

Preview a change without writing files:

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\CodexProfileSwitcher.ps1 activate -Profile chatgpt -WhatIf
```

## Notes

- This does not merge ChatGPT cloud history with API-key-based usage. It preserves one shared local history store on this machine.
- Close Codex Desktop before `save` or `activate` for the most reliable results.
- Use `-Force` only when you want to overwrite an existing saved profile or bypass the basic process-name check.
