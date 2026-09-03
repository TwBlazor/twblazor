# Shared helper for the two version scripts. Dot-source it:
#
#   . "$PSScriptRoot/version-tags.ps1"
#
# Releases are recorded only as Git tags, so both next-release-version.ps1 and
# next-preview-version.ps1 need the same answer to "what was released last?".
# That lookup lives here so the two cannot drift apart.

Set-StrictMode -Version Latest

function Get-LatestReleaseTag {
    <#
    .SYNOPSIS
        Returns the highest stable release tag in the repository, or $null.

    .DESCRIPTION
        Only stable "<prefix>major.minor.patch" tags count; prerelease tags such
        as v1.2.0-rc.1 and anything that is not a version are ignored.

        Reachability is deliberately not considered. Releases are tagged on
        main's merge commit, which never joins develop's linear history, so a
        reachability-based lookup would hide the newest release from develop.

        Sorting happens here rather than via git's --sort=v:refname so that the
        comparison is numeric per component: v1.10.0 must outrank v1.9.0.

    .OUTPUTS
        PSCustomObject with Major, Minor, Patch and Tag, or $null when the
        repository has no stable release tag yet.
    #>
    [CmdletBinding()]
    param(
        [string]$TagPrefix = 'v'
    )

    $pattern = "^$([regex]::Escape($TagPrefix))(\d+)\.(\d+)\.(\d+)$"

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
        return $null
    }

    return $releases[-1]
}

function Set-GitHubOutput {
    <#
    .SYNOPSIS
        Appends name=value to $GITHUB_OUTPUT when running under GitHub Actions.
        Does nothing locally, so both scripts can be run by hand.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string]$Name,
        [Parameter(Mandatory)][string]$Value
    )

    if ($env:GITHUB_OUTPUT) {
        "$Name=$Value" | Out-File -FilePath $env:GITHUB_OUTPUT -Append -Encoding utf8
    }
}
