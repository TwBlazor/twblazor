# Reports repeated Tailwind utility classes in a C# theme file.
#
# Usage:
#   pwsh ./scripts/analyze-theme-classes.ps1
#   pwsh ./scripts/analyze-theme-classes.ps1 -Path src/TwBlazor.Theme/Theme.cs -MinimumCount 4
#   pwsh ./scripts/analyze-theme-classes.ps1 -StrongCandidateCount 10
#   pwsh ./scripts/analyze-theme-classes.ps1 -CsvPath theme-classes.csv

[CmdletBinding()]
param(
    [string]$Path = 'src/TwBlazor.Theme/Theme.cs',
    [ValidateRange(2, 1000)][int]$MinimumCount = 3,
    [ValidateRange(2, 1000)][int]$StrongCandidateCount = 8,
    [ValidateRange(1, 10000)][int]$Top = 50,
    [string]$CsvPath
)

$ErrorActionPreference = 'Stop'

if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
    throw "Theme file was not found: $Path"
}

$resolvedPath = (Resolve-Path -LiteralPath $Path).Path
$lines = [System.IO.File]::ReadAllLines($resolvedPath)
$classOccurrences = [System.Collections.Generic.List[object]]::new()

# This intentionally reads only C# string literals, so identifiers and comments
# do not inflate the utility counts.
$stringPattern = '(?<!\\)"(?:\\.|[^"\\])*"'
$classPattern = '^[^\s{}$]+$'

for ($lineIndex = 0; $lineIndex -lt $lines.Count; $lineIndex++) {
    $line = $lines[$lineIndex]
    foreach ($stringMatch in [regex]::Matches($line, $stringPattern)) {
        $literal = $stringMatch.Value.Substring(1, $stringMatch.Value.Length - 2)
        $literal = $literal -replace '\\"', '"' -replace '\\\\', '\\'

        foreach ($token in ($literal -split '\s+')) {
            if ($token -match $classPattern -and $token -match '[A-Za-z]') {
                $classOccurrences.Add([pscustomobject]@{
                    ClassName = $token
                    Line = $lineIndex + 1
                })
            }
        }
    }
}

$report = @(
    $classOccurrences |
        Group-Object ClassName |
        Where-Object Count -ge $MinimumCount |
        Sort-Object Count, Name -Descending |
        Select-Object -First $Top |
        ForEach-Object {
            $locations = @(
                $classOccurrences |
                    Where-Object ClassName -eq $_.Name |
                    Select-Object -ExpandProperty Line -Unique
            )

            [pscustomobject]@{
                Count = $_.Count
                ClassName = $_.Name
                Recommendation = if ($_.Count -ge $StrongCandidateCount) { 'Strong candidate' } else { 'Review' }
                Lines = $locations -join ', '
            }
        }
)

if ($report.Count -eq 0) {
    Write-Host "No classes occur at least $MinimumCount times in $Path."
    exit 0
}

Write-Host "Repeated classes in $Path (minimum count: $MinimumCount)"
Write-Host "Review candidates with high counts and related semantics for extraction into a shared utility."
Write-Host ''
$report | Format-Table -AutoSize

if ($CsvPath) {
    $report | Export-Csv -LiteralPath $CsvPath -NoTypeInformation
    Write-Host "CSV report written to $CsvPath"
}