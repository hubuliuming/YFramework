[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [Parameter(Position = 0)]
    [ValidateSet("status", "list", "save", "activate")]
    [string]$Action = "status",

    [string]$Profile,

    [string]$CodexHome = $(if ($env:CODEX_HOME) { $env:CODEX_HOME } else { Join-Path $HOME ".codex" }),

    [switch]$Force
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$script:TrackedPatterns = @(
    "auth.json",
    "config.toml",
    ".codex-global-state.json",
    "cap_sid",
    "state_*.sqlite",
    "state_*.sqlite-shm",
    "state_*.sqlite-wal"
)

$script:SharedHistoryPaths = @(
    "sessions",
    "archived_sessions",
    "session_index.jsonl"
)

function Get-ProfileStoreRoot {
    return Join-Path $CodexHome "profile-switcher\profiles"
}

function Get-ProfilePath {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Name
    )

    return Join-Path (Get-ProfileStoreRoot) $Name
}

function Assert-CodexHomeExists {
    if (-not (Test-Path -LiteralPath $CodexHome -PathType Container)) {
        throw "Codex home does not exist: $CodexHome"
    }
}

function Assert-ProfileProvided {
    if ([string]::IsNullOrWhiteSpace($Profile)) {
        throw "Profile name is required for action '$Action'."
    }
}

function Assert-CodexClosed {
    $matches = @(Get-Process -ErrorAction SilentlyContinue | Where-Object { $_.ProcessName -match "codex" })
    if ($matches.Count -gt 0 -and -not $Force) {
        $names = ($matches | Select-Object -ExpandProperty ProcessName -Unique) -join ", "
        throw "Detected running Codex process(es): $names. Close Codex Desktop first, or rerun with -Force if you know the files are safe to copy."
    }

    if ($matches.Count -gt 0) {
        Write-Warning "Continuing while Codex-related process(es) appear to be running. File copies can fail if the app still holds locks."
    }
}

function Get-TrackedFilesFromCodexHome {
    $files = New-Object System.Collections.Generic.List[System.IO.FileInfo]

    foreach ($pattern in $script:TrackedPatterns) {
        $matches = @(Get-ChildItem -Path (Join-Path $CodexHome $pattern) -File -Force -ErrorAction SilentlyContinue)
        foreach ($match in $matches) {
            $files.Add($match)
        }
    }

    return @($files | Sort-Object FullName -Unique)
}

function Get-TrackedFilesFromProfile {
    param(
        [Parameter(Mandatory = $true)]
        [string]$ProfilePath
    )

    if (-not (Test-Path -LiteralPath $ProfilePath -PathType Container)) {
        return @()
    }

    return @(Get-ChildItem -Path $ProfilePath -Recurse -File -Force | Where-Object { $_.Name -ne "manifest.json" } | Sort-Object FullName)
}

function Get-RelativePath {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Root,
        [Parameter(Mandatory = $true)]
        [string]$Path
    )

    return $Path.Substring($Root.Length).TrimStart("\")
}

function New-DirectoryIfMissing {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Path
    )

    if (-not (Test-Path -LiteralPath $Path -PathType Container)) {
        New-Item -ItemType Directory -Path $Path -Force | Out-Null
    }
}

function Save-ProfileSnapshot {
    Assert-ProfileProvided
    Assert-CodexClosed

    $profilePath = Get-ProfilePath -Name $Profile
    if (Test-Path -LiteralPath $profilePath) {
        if (-not $Force) {
            throw "Profile already exists: $Profile. Rerun with -Force to overwrite it."
        }

        if ($PSCmdlet.ShouldProcess($profilePath, "Remove existing profile snapshot")) {
            Remove-Item -LiteralPath $profilePath -Recurse -Force
        }
    }

    if ($PSCmdlet.ShouldProcess($profilePath, "Create profile snapshot directory")) {
        New-DirectoryIfMissing -Path $profilePath
    }

    $trackedFiles = @(Get-TrackedFilesFromCodexHome)
    $copiedFiles = New-Object System.Collections.Generic.List[string]

    foreach ($file in $trackedFiles) {
        $relativePath = Get-RelativePath -Root $CodexHome -Path $file.FullName
        $targetPath = Join-Path $profilePath $relativePath
        $targetDir = Split-Path -Path $targetPath -Parent

        if ($PSCmdlet.ShouldProcess($targetPath, "Copy '$relativePath' into profile '$Profile'")) {
            New-DirectoryIfMissing -Path $targetDir
            Copy-Item -LiteralPath $file.FullName -Destination $targetPath -Force
        }

        $copiedFiles.Add($relativePath)
    }

    $manifest = [ordered]@{
        profile            = $Profile
        savedAt            = (Get-Date).ToString("o")
        codexHome          = $CodexHome
        snapshotFiles      = @($copiedFiles)
        sharedHistoryPaths = $script:SharedHistoryPaths
        note               = "Shared history stays in the live Codex home. This snapshot intentionally excludes session history."
    }

    $manifestPath = Join-Path $profilePath "manifest.json"
    if ($PSCmdlet.ShouldProcess($manifestPath, "Write profile manifest")) {
        $manifest | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath $manifestPath -Encoding utf8
    }

    Write-Host "Saved profile '$Profile' with $($copiedFiles.Count) tracked file(s)." -ForegroundColor Green
    Write-Host "Shared history remains in: $CodexHome" -ForegroundColor Green
}

function Activate-ProfileSnapshot {
    Assert-ProfileProvided
    Assert-CodexClosed

    $profilePath = Get-ProfilePath -Name $Profile
    if (-not (Test-Path -LiteralPath $profilePath -PathType Container)) {
        throw "Profile does not exist: $Profile"
    }

    $currentTrackedFiles = @(Get-TrackedFilesFromCodexHome)
    foreach ($file in $currentTrackedFiles) {
        if ($PSCmdlet.ShouldProcess($file.FullName, "Remove current tracked auth/state file before activation")) {
            Remove-Item -LiteralPath $file.FullName -Force
        }
    }

    $profileFiles = @(Get-TrackedFilesFromProfile -ProfilePath $profilePath)
    foreach ($file in $profileFiles) {
        $relativePath = Get-RelativePath -Root $profilePath -Path $file.FullName
        $targetPath = Join-Path $CodexHome $relativePath
        $targetDir = Split-Path -Path $targetPath -Parent

        if ($PSCmdlet.ShouldProcess($targetPath, "Restore '$relativePath' from profile '$Profile'")) {
            New-DirectoryIfMissing -Path $targetDir
            Copy-Item -LiteralPath $file.FullName -Destination $targetPath -Force
        }
    }

    Write-Host "Activated profile '$Profile'." -ForegroundColor Green
    Write-Host "Shared history was preserved: $($script:SharedHistoryPaths -join ', ')" -ForegroundColor Green
}

function Get-ConfigValue {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Path,
        [Parameter(Mandatory = $true)]
        [string]$Key
    )

    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        return $null
    }

    $match = Select-String -Path $Path -Pattern ("^\s*" + [regex]::Escape($Key) + "\s*=\s*""?([^""]+)""?\s*$") | Select-Object -First 1
    if ($null -eq $match) {
        return $null
    }

    return $match.Matches[0].Groups[1].Value
}

function Show-Status {
    $authPath = Join-Path $CodexHome "auth.json"
    $configPath = Join-Path $CodexHome "config.toml"
    $globalStatePath = Join-Path $CodexHome ".codex-global-state.json"
    $sessionsPath = Join-Path $CodexHome "sessions"
    $profilesRoot = Get-ProfileStoreRoot

    $authKeys = @()
    if (Test-Path -LiteralPath $authPath -PathType Leaf) {
        try {
            $auth = Get-Content -LiteralPath $authPath -Raw | ConvertFrom-Json
            $authKeys = @($auth.PSObject.Properties.Name)
        }
        catch {
            $authKeys = @("<unreadable>")
        }
    }

    $cloudAccess = $null
    if (Test-Path -LiteralPath $globalStatePath -PathType Leaf) {
        try {
            $globalState = Get-Content -LiteralPath $globalStatePath -Raw | ConvertFrom-Json
            $cloudAccess = $globalState."electron-persisted-atom-state".codexCloudAccess
        }
        catch {
            $cloudAccess = "<unreadable>"
        }
    }

    $profileCount = 0
    if (Test-Path -LiteralPath $profilesRoot -PathType Container) {
        $profileCount = @(Get-ChildItem -LiteralPath $profilesRoot -Directory -Force).Count
    }

    $sessionCount = 0
    if (Test-Path -LiteralPath $sessionsPath -PathType Container) {
        $sessionCount = @(Get-ChildItem -LiteralPath $sessionsPath -Recurse -File -Force).Count
    }

    [PSCustomObject]@{
        CodexHome           = $CodexHome
        ProfileStore        = $profilesRoot
        SavedProfiles       = $profileCount
        SharedSessionFiles  = $sessionCount
        AuthKeys            = if ($authKeys.Count -gt 0) { $authKeys -join ", " } else { "<none>" }
        PreferredAuthMethod = Get-ConfigValue -Path $configPath -Key "preferred_auth_method"
        RequiresOpenAIAuth  = Get-ConfigValue -Path $configPath -Key "requires_openai_auth"
        CodexCloudAccess    = if ($cloudAccess) { $cloudAccess } else { "<unknown>" }
        SharedHistoryPaths  = $script:SharedHistoryPaths -join ", "
    } | Format-List
}

function Show-Profiles {
    $profilesRoot = Get-ProfileStoreRoot
    if (-not (Test-Path -LiteralPath $profilesRoot -PathType Container)) {
        Write-Host "No saved profiles yet." -ForegroundColor Yellow
        return
    }

    $rows = foreach ($directory in Get-ChildItem -LiteralPath $profilesRoot -Directory -Force | Sort-Object Name) {
        $manifestPath = Join-Path $directory.FullName "manifest.json"
        $savedAt = $null
        $snapshotFiles = @()

        if (Test-Path -LiteralPath $manifestPath -PathType Leaf) {
            try {
                $manifest = Get-Content -LiteralPath $manifestPath -Raw | ConvertFrom-Json
                $savedAt = $manifest.savedAt
                $snapshotFiles = @($manifest.snapshotFiles)
            }
            catch {
                $savedAt = "<unreadable>"
            }
        }

        [PSCustomObject]@{
            Profile       = $directory.Name
            SavedAt       = if ($savedAt) { $savedAt } else { "<unknown>" }
            SnapshotFiles = $snapshotFiles.Count
        }
    }

    if (@($rows).Count -eq 0) {
        Write-Host "No saved profiles yet." -ForegroundColor Yellow
        return
    }

    $rows | Format-Table -AutoSize
}

try {
    Assert-CodexHomeExists

    switch ($Action) {
        "status"   { Show-Status }
        "list"     { Show-Profiles }
        "save"     { Save-ProfileSnapshot }
        "activate" { Activate-ProfileSnapshot }
        default    { throw "Unsupported action: $Action" }
    }
}
catch {
    Write-Error $_
    exit 1
}
