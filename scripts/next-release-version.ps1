# Computes the next stable release version from the highest existing release
# tag, given a bump kind. Versions live only in Git tags (MinVer derives the
# package version from them), so this is the single place that decides what a
# release is numbered.
#
#   -Bump Minor (default) -> 1.1.0 becomes 1.2.0
#   -Bump Major           -> 1.1.0 becomes 2.0.0
#
# Prerelease tags are ignored when picking the base; only stable
# "<prefix>major.minor.patch" tags count. When the repository has no release tag
# at all the base is 0.0.0, so the first Minor release is 0.1.0.
#
# Writes "version" and "tag" to $GITHUB_OUTPUT when running under Actions.
#
# Usage:
#   pwsh ./scripts/next-release-version.ps1 [-Bump Minor|Major] [-TagPrefix v]

[CmdletBinding()]
param(
    [ValidateSet('Minor', 'Major')]
    [string]$Bump = 'Minor',
    [string]$TagPrefix = 'v'
)

$ErrorActionPreference = 'Stop'

$pattern = "^$([regex]::Escape($TagPrefix))(\d+)\.(\d+)\.(\d+)$"

# Sort in PowerShell rather than relying on git's --sort=v:refname, so the
# comparison is numeric per-component regardless of the Git version available.
$releases = @(
    git tag --list "$TagPrefix*" |
        ForEach-Object {
            if ($_ -match $pattern) {
                [PSCustomObject]@{
                    Major = [int]$Matches[1]
                    Minor = [int]$Matches[2]
                    Patch = [int]$Matches[3]
                    Tag   = $_
                }
            }
        } |
        Sort-Object Major, Minor, Patch
)

if ($releases.Count -eq 0) {
    Write-Host "No stable '$TagPrefix<major>.<minor>.<patch>' tags found; treating the base version as 0.0.0."
    $base = [PSCustomObject]@{ Major = 0; Minor = 0; Patch = 0; Tag = '(none)' }
} else {
    $base = $releases[-1]
    Write-Host "Latest release tag: $($base.Tag)"
}

if ($Bump -eq 'Major') {
    $version = "$($base.Major + 1).0.0"
} else {
    $version = "$($base.Major).$($base.Minor + 1).0"
}

$tag = "$TagPrefix$version"
Write-Host "$Bump bump -> $version (tag $tag)"

if ($env:GITHUB_OUTPUT) {
    "version=$version" | Out-File -FilePath $env:GITHUB_OUTPUT -Append -Encoding utf8
    "tag=$tag"         | Out-File -FilePath $env:GITHUB_OUTPUT -Append -Encoding utf8
}

Write-Output $version
