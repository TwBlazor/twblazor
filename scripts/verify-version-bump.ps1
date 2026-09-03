# Verifies that version.json's "version" field was bumped relative to a base
# ref, and can auto-bump it. Two modes:
#
#   -Component Minor (default) - used for develop -> main. Requires the head
#     version's major.minor to exceed the base's; on -Fix, rewrites version.json
#     to "base.major.(base.minor+1).0" (a clean, non-prerelease release version,
#     discarding whatever prerelease/patch state the head was at).
#
#   -Component Patch - used for any-branch -> develop. Requires the head
#     version's major.minor.patch to exceed the base's (same major.minor, patch
#     at least +1); on -Fix, rewrites version.json to
#     "base.major.base.minor.(base.patch+1)-beta".
#
# With -Fix, if the bump is missing, rewrites version.json in place (in
# HeadRef's working copy) instead of failing, so a workflow can commit the
# change automatically. Without -Fix, exits 1 if the bump is missing.
#
# Usage:
#   pwsh ./scripts/verify-version-bump.ps1 [-BaseRef origin/main] [-HeadRef HEAD] [-Component Minor|Patch] [-Fix]

[CmdletBinding()]
param(
    [string]$BaseRef = 'origin/main',
    [string]$HeadRef = 'HEAD',
    [ValidateSet('Minor', 'Patch')]
    [string]$Component = 'Minor',
    [switch]$Fix
)

$ErrorActionPreference = 'Stop'

function Get-VersionJson {
    param([string]$Ref)

    $json = git show "${Ref}:version.json" 2>$null
    if (-not $json) {
        throw "Could not read version.json at ref '$Ref'."
    }
    return $json | ConvertFrom-Json
}

function ConvertTo-SemVerParts {
    param([string]$VersionString)

    # Split off any prerelease tag (everything from the first '-' onward) before
    # parsing the numeric core, e.g. "1.0.5-beta" -> core "1.0.5", tag "beta".
    $core = $VersionString.Split('-', 2)[0]
    $parts = $core.Split('.')
    if ($parts.Count -lt 2) {
        throw "Unexpected version format '$VersionString'; expected at least 'major.minor'."
    }
    return [PSCustomObject]@{
        Major = [int]$parts[0]
        Minor = [int]$parts[1]
        Patch = if ($parts.Count -ge 3) { [int]$parts[2] } else { 0 }
    }
}

$baseVersionJson = Get-VersionJson -Ref $BaseRef
$headVersionJson = Get-VersionJson -Ref $HeadRef

$base = ConvertTo-SemVerParts $baseVersionJson.version
$head = ConvertTo-SemVerParts $headVersionJson.version

Write-Host "base ($BaseRef) version.json version: $($baseVersionJson.version)"
Write-Host "head ($HeadRef) version.json version: $($headVersionJson.version)"

if ($Component -eq 'Minor') {
    $ok = ($head.Major -gt $base.Major) -or
          ($head.Major -eq $base.Major -and $head.Minor -ge $base.Minor + 1)
    $newVersionString = "$($base.Major).$($base.Minor + 1).0"
    $failHint = "Bump the 'version' field's minor number (or major number) in version.json before merging to main."
} else {
    $ok = ($head.Major -gt $base.Major) -or
          ($head.Major -eq $base.Major -and $head.Minor -gt $base.Minor) -or
          ($head.Major -eq $base.Major -and $head.Minor -eq $base.Minor -and $head.Patch -ge $base.Patch + 1)
    $newVersionString = "$($base.Major).$($base.Minor).$($base.Patch + 1)-beta"
    $failHint = "Bump the 'version' field's patch number in version.json before merging to develop."
}

if ($ok) {
    Write-Host "$Component bump requirement satisfied." -ForegroundColor Green
    exit 0
}

if (-not $Fix) {
    Write-Host "version.json was not sufficiently bumped." -ForegroundColor Red
    Write-Host $failHint -ForegroundColor Yellow
    Write-Host "  base: $($baseVersionJson.version)"
    Write-Host "  head: $($headVersionJson.version)"
    exit 1
}

# -Fix: rewrite version.json in the working copy. This only makes sense when
# HeadRef is the working copy (i.e. 'HEAD'); the caller commits the change.
$path = 'version.json'
$content = [System.IO.File]::ReadAllText($path)
$newContent = $content -replace '("version"\s*:\s*")[^"]+(")', "`${1}$newVersionString`${2}"

if ($newContent -eq $content) {
    throw "Could not find a 'version' field to update in $path."
}

[System.IO.File]::WriteAllText($path, $newContent, (New-Object System.Text.UTF8Encoding($false)))
Write-Host "Bumped version.json: $($headVersionJson.version) -> $newVersionString" -ForegroundColor Green
