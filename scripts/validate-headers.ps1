# Validates that every .cs file in the TwBlazor library project (TwBlazor.csproj)
# starts with the MIT copyright header defined in .editorconfig's file_header_template.
#
# Usage:
#   pwsh ./scripts/validate-headers.ps1        - Report files missing the header (exit 1 if any)
#   pwsh ./scripts/validate-headers.ps1 -Fix    - Insert the header into files that are missing it

[CmdletBinding()]
param(
    [switch]$Fix
)

$ErrorActionPreference = 'Stop'

$repoRoot = (git rev-parse --show-toplevel).Trim()
Push-Location $repoRoot
try {
    $headerLines = @(
        '// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com'
        '// Licensed under the MIT License. See LICENSE.txt in the project root for license information.'
    )

    # Only TwBlazor.csproj ships as the published package - other projects (Docs, Tests,
    # Theme, BuildTools, etc.) don't need the header.
    $files = git ls-files 'TwBlazor/*.cs' | Where-Object { $_ -notmatch '[\\/](obj|bin)[\\/]' }

    $utf8NoBom = New-Object System.Text.UTF8Encoding($false)
    $missing = @()

    foreach ($relativePath in $files) {
        $fullPath = Join-Path $repoRoot $relativePath
        $bytes = [System.IO.File]::ReadAllBytes($fullPath)
        if ($bytes.Length -eq 0) { continue }

        $hasBom = $bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF
        $offset = if ($hasBom) { 3 } else { 0 }
        $content = $utf8NoBom.GetString($bytes, $offset, $bytes.Length - $offset)

        if ($content.StartsWith($headerLines[0])) {
            continue
        }

        $missing += $relativePath

        if ($Fix) {
            $newline = if ($content.Contains("`r`n")) { "`r`n" } else { "`n" }
            $header = ($headerLines -join $newline) + $newline + $newline
            $newContent = $header + $content.TrimStart("`r", "`n")

            $outBytes = $utf8NoBom.GetBytes($newContent)
            if ($hasBom) {
                $outBytes = [byte[]](0xEF, 0xBB, 0xBF) + $outBytes
            }
            [System.IO.File]::WriteAllBytes($fullPath, $outBytes)
        }
    }

    if ($missing.Count -eq 0) {
        Write-Host "All $($files.Count) file(s) in TwBlazor/ have the copyright header." -ForegroundColor Green
        exit 0
    }

    if ($Fix) {
        Write-Host "Added the copyright header to $($missing.Count) file(s):" -ForegroundColor Yellow
        $missing | ForEach-Object { Write-Host "  $_" }
        exit 0
    }

    Write-Host "Missing copyright header in $($missing.Count) file(s):" -ForegroundColor Red
    $missing | ForEach-Object { Write-Host "  $_" }
    Write-Host ""
    Write-Host "Run 'pwsh ./scripts/validate-headers.ps1 -Fix' to add the missing header(s)." -ForegroundColor Yellow
    exit 1
}
finally {
    Pop-Location
}
