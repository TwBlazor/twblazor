# TwBlazor Tailwind CSS Build Script
# Usage:
#   .\watch-tailwind.ps1           - Dev mode: watches for changes (unminified)
#   .\watch-tailwind.ps1 -Release  - Release mode: one-time minified build, then exits
#
# In development, ASP.NET Core's static web assets middleware serves _content/TwBlazor/css/twblazor.css
# directly from TwBlazor\wwwroot\css\twblazor.css (source directory).

param(
    [switch]$Release
)

if ($Release) {
    Write-Host "Starting TwBlazor Tailwind Release Build (minified)..." -ForegroundColor Green
} else {
    Write-Host "Starting TwBlazor Tailwind Development Watcher..." -ForegroundColor Green
}
Write-Host ""

# Define paths
$twBlazorInput = ".\TwBlazor\wwwroot\css\input.css"
$twBlazorOutput = ".\TwBlazor\wwwroot\css\twblazor.css"
$twDocsInput = ".\TwBlazor.Docs\wwwroot\css\input.css"
$twDocsOutput = ".\TwBlazor.Docs\wwwroot\css\output.css"

# Verify paths exist
if (-not (Test-Path $twBlazorInput)) {
    Write-Host "ERROR: TwBlazor input.css not found at $twBlazorInput" -ForegroundColor Red
    exit 1
}
if (-not (Test-Path $twDocsInput)) {
    Write-Host "ERROR: TwBlazor.Docs input.css not found at $twDocsInput" -ForegroundColor Red
    exit 1
}

# Initial build
Write-Host "Running initial Tailwind builds..." -ForegroundColor Yellow
$minifyArgs = if ($Release) { @("--minify") } else { @() }
Write-Host "Building TwBlazor..." -ForegroundColor Cyan
npx @tailwindcss/cli -i $twBlazorInput -o $twBlazorOutput @minifyArgs
Write-Host "Building TwBlazor.Docs..." -ForegroundColor Cyan
npx @tailwindcss/cli -i $twDocsInput -o $twDocsOutput @minifyArgs
Write-Host "Initial builds complete!" -ForegroundColor Green
Write-Host ""

if ($Release) {
    Write-Host "Release build complete. Minified output written to:" -ForegroundColor Green
    Write-Host "  $twBlazorOutput" -ForegroundColor Cyan
    Write-Host "  $twDocsOutput" -ForegroundColor Cyan
    exit 0
}

# Start Tailwind watchers as background processes
Write-Host "Starting Tailwind CLI watchers..." -ForegroundColor Yellow

$env:FORCE_COLOR = "0"  # Disable color codes for tailwind output

# TwBlazor watcher
$twBlazorProcess = Start-Process -FilePath "cmd.exe" -ArgumentList "/c npx @tailwindcss/cli -i $twBlazorInput -o $twBlazorOutput --watch" -NoNewWindow -PassThru

# TwBlazor.Docs watcher
$twDocsProcess = Start-Process -FilePath "cmd.exe" -ArgumentList "/c npx @tailwindcss/cli -i $twDocsInput -o $twDocsOutput --watch" -NoNewWindow -PassThru

Start-Sleep -Seconds 2  # Give processes time to start

if (-not $twBlazorProcess.HasExited) {
    Write-Host "[OK] TwBlazor watcher started (PID: $($twBlazorProcess.Id))" -ForegroundColor Green
} else {
    Write-Host "[WARN] TwBlazor watcher exited with code $($twBlazorProcess.ExitCode)" -ForegroundColor Red
}
if (-not $twDocsProcess.HasExited) {
    Write-Host "[OK] TwBlazor.Docs watcher started (PID: $($twDocsProcess.Id))" -ForegroundColor Green
} else {
    Write-Host "[WARN] TwBlazor.Docs watcher exited with code $($twDocsProcess.ExitCode)" -ForegroundColor Red
}
Write-Host ""
Write-Host "================================" -ForegroundColor Magenta
Write-Host "  Watching for changes..." -ForegroundColor Magenta
Write-Host "  Refresh browser (Ctrl+F5) to see updates" -ForegroundColor Magenta
Write-Host "  Press Ctrl+C to stop" -ForegroundColor Magenta
Write-Host "================================" -ForegroundColor Magenta
Write-Host ""

# Keep script running and monitor processes
try {
    while ($true) {
        # Check if processes are still running
        if ($twBlazorProcess.HasExited) {
            Write-Host "TwBlazor watcher exited (code: $($twBlazorProcess.ExitCode))! Restarting..." -ForegroundColor Red
            $twBlazorProcess = Start-Process -FilePath "cmd.exe" -ArgumentList "/c npx @tailwindcss/cli -i $twBlazorInput -o $twBlazorOutput --watch" -NoNewWindow -PassThru
        }
        if ($twDocsProcess.HasExited) {
            Write-Host "TwBlazor.Docs watcher exited (code: $($twDocsProcess.ExitCode))! Restarting..." -ForegroundColor Red
            $twDocsProcess = Start-Process -FilePath "cmd.exe" -ArgumentList "/c npx @tailwindcss/cli -i $twDocsInput -o $twDocsOutput --watch" -NoNewWindow -PassThru
        }

        Start-Sleep -Seconds 2
    }
}
finally {
    # Cleanup
    Write-Host ""
    Write-Host "Stopping watchers..." -ForegroundColor Yellow

    # Kill process trees (cmd.exe -> node) to avoid orphaned processes
    foreach ($proc in @($twBlazorProcess, $twDocsProcess)) {
        if ($proc -and -not $proc.HasExited) {
            taskkill /PID $proc.Id /T /F 2>$null | Out-Null
        }
    }

    Write-Host "[OK] Cleanup complete" -ForegroundColor Green
}
