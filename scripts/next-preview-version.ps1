# Computes develop's prerelease version from the highest release tag in the
# repository, without requiring that tag to be reachable from HEAD.
#
# MinVer normally numbers a build from the newest tag *reachable* from the commit
# being built. That does not suit this repository: releases are tagged on main's
# merge commit, which never becomes part of develop's history, because develop
# keeps a linear history and takes no back-merges. Left to itself MinVer would
# keep numbering develop's previews from the previous release, below the version
# already shipped.
#
# "<tag>..HEAD" is well defined even when <tag> is not an ancestor of HEAD - it
# counts the commits on develop that the release does not already contain - so
# the height stays correct without any merge:
#
#   v1.2.0 released, develop untouched -> 1.2.1-preview.0
#   one commit on develop              -> 1.2.1-preview.1
#   another commit                     -> 1.2.1-preview.2
#
# The result is fed to MinVer as MinVerVersionOverride so that the assembly,
# file and package versions all agree.
#
# Writes "version" to $GITHUB_OUTPUT when running under Actions.
#
# Usage:
#   pwsh ./scripts/next-preview-version.ps1 [-TagPrefix v] [-PreReleaseTag preview]

[CmdletBinding()]
param(
    [string]$TagPrefix = 'v',
    [string]$PreReleaseTag = 'preview'
)

$ErrorActionPreference = 'Stop'

. "$PSScriptRoot/version-tags.ps1"

$base = Get-LatestReleaseTag -TagPrefix $TagPrefix

if ($null -eq $base) {
    Write-Host "No stable '$TagPrefix<major>.<minor>.<patch>' tags found; treating the base version as 0.0.0."
    $base = [PSCustomObject]@{ Major = 0; Minor = 0; Patch = 0; Tag = $null }
    $height = @(git rev-list --count HEAD)[0]
} else {
    Write-Host "Latest release tag: $($base.Tag)"
    $height = @(git rev-list --count "$($base.Tag)..HEAD")[0]
}

Write-Host "Commits on this branch not already in that release: $height"

$version = "$($base.Major).$($base.Minor).$($base.Patch + 1)-$PreReleaseTag.$height"
Write-Host "Preview version -> $version"

Set-GitHubOutput -Name 'version' -Value $version

Write-Output $version
