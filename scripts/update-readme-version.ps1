# Rewrites the install command in README.md's Setup section so it always names
# the newest published release exactly:
#
#   dotnet add package TwBlazor --version 1.3.2
#
# Every release moves it, patches included, so the README never names a version
# older than what is on NuGet.
#
# Versions live only in Git tags, so the release line comes from the newest
# stable tag through the shared helper - the same lookup the publish workflows
# use, so the README cannot name a line that was never released.
#
# -Version names the line explicitly, for the release workflow: at that point
# the version being cut has been decided but not yet tagged, so the tag lookup
# would still answer with the *previous* release.
#
# Writes "version" and "changed" to $GITHUB_OUTPUT when running under Actions.
#
# Usage:
#   pwsh ./scripts/update-readme-version.ps1 [-Path README.md] [-TagPrefix v]
#   pwsh ./scripts/update-readme-version.ps1 -Version 1.4.0

[CmdletBinding()]
param(
    [string]$Path = "$PSScriptRoot/../README.md",
    [string]$TagPrefix = 'v',
    [string]$Version
)

$ErrorActionPreference = 'Stop'

. "$PSScriptRoot/version-tags.ps1"

if ($Version) {
    if ($Version -notmatch '^(\d+)\.(\d+)\.\d+') {
        throw "-Version must look like <major>.<minor>.<patch>, got '$Version'."
    }

    $installVersion = $Version
    Write-Host "Release version $Version -> --version $installVersion"
} else {
    $release = Get-LatestReleaseTag -TagPrefix $TagPrefix

    if ($null -eq $release) {
        Write-Host "No stable '$TagPrefix<major>.<minor>.<patch>' tag found; leaving the README unchanged."
        Set-GitHubOutput -Name 'changed' -Value 'false'
        return
    }

    $installVersion = "$($release.Major).$($release.Minor).$($release.Patch)"
    Write-Host "Latest release tag: $($release.Tag) -> --version $installVersion"
}

$readme = Get-Content -Path $Path -Raw

# Deliberately matches whatever the line carries today - the x.x.x placeholder,
# a pinned version, or an older floating one - so re-running is a no-op.
$pattern = '(dotnet add package TwBlazor --version )\S+'
$found = [regex]::Matches($readme, $pattern)

# A silent no-op would be worse than failing: if the install command is ever
# reworded, this script must say so rather than quietly stop maintaining it.
if ($found.Count -ne 1) {
    throw "Expected exactly one 'dotnet add package TwBlazor --version <version>' line in $Path, found $($found.Count). Update `$pattern in this script if the install command was reworded."
}

$updated = [regex]::Replace($readme, $pattern, "`${1}$installVersion")

Set-GitHubOutput -Name 'version' -Value $installVersion

if ($updated -eq $readme) {
    Write-Host "README already reads '--version $installVersion'; nothing to do."
    Set-GitHubOutput -Name 'changed' -Value 'false'
    return
}

# -NoNewline because $readme was read with -Raw and already ends in whatever
# trailing newline the file had; Set-Content would otherwise append another.
Set-Content -Path $Path -Value $updated -NoNewline -Encoding utf8

Write-Host "Updated $Path to '--version $installVersion'."
Set-GitHubOutput -Name 'changed' -Value 'true'
