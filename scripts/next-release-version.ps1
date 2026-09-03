# Computes the next stable release version from the highest existing release
# tag, given a bump kind. Versions live only in Git tags (MinVer derives the
# package version from them), so this is the single place that decides what a
# release is numbered.
#
#   -Bump Minor (default) -> 1.1.0 becomes 1.2.0
#   -Bump Major           -> 1.1.0 becomes 2.0.0
#
# When the repository has no release tag at all the base is 0.0.0, so the first
# Minor release is 0.1.0.
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

. "$PSScriptRoot/version-tags.ps1"

$base = Get-LatestReleaseTag -TagPrefix $TagPrefix

if ($null -eq $base) {
    Write-Host "No stable '$TagPrefix<major>.<minor>.<patch>' tags found; treating the base version as 0.0.0."
    $base = [PSCustomObject]@{ Major = 0; Minor = 0; Patch = 0; Tag = $null }
} else {
    Write-Host "Latest release tag: $($base.Tag)"
}

if ($Bump -eq 'Major') {
    $version = "$($base.Major + 1).0.0"
} else {
    $version = "$($base.Major).$($base.Minor + 1).0"
}

$tag = "$TagPrefix$version"
Write-Host "$Bump bump -> $version (tag $tag)"

Set-GitHubOutput -Name 'version' -Value $version
Set-GitHubOutput -Name 'tag'     -Value $tag

Write-Output $version
