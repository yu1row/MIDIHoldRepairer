#Requires -Version 5.1
<#
.SYNOPSIS
  Build docs/README.pdf and docs/README-ja.pdf from the Markdown READMEs.
#>
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
Set-Location $repoRoot

if (-not (Get-Command node -ErrorAction SilentlyContinue)) {
    throw "Node.js is required to build PDF manuals. Install Node.js and retry."
}

function Find-BrowserPath {
    $candidates = @(
        "$env:ProgramFiles\Google\Chrome\Application\chrome.exe",
        "${env:ProgramFiles(x86)}\Google\Chrome\Application\chrome.exe",
        "$env:ProgramFiles\Microsoft\Edge\Application\msedge.exe",
        "${env:ProgramFiles(x86)}\Microsoft\Edge\Application\msedge.exe"
    )
    foreach ($path in $candidates) {
        if ($path -and (Test-Path $path)) {
            return $path
        }
    }
    return $null
}

function New-PdfSourceMarkdown {
    param(
        [Parameter(Mandatory = $true)][string]$SourcePath,
        [Parameter(Mandatory = $true)][string]$DestinationPath
    )

    $lines = Get-Content -LiteralPath $SourcePath -Encoding UTF8
    $start = 0
    if ($lines.Count -gt 0 -and $lines[0] -match 'English' -and $lines[0] -match '日本語') {
        $start = 1
        while ($start -lt $lines.Count -and [string]::IsNullOrWhiteSpace($lines[$start])) {
            $start++
        }
    }

    $filtered = @()
    if ($start -lt $lines.Count) {
        $filtered = $lines[$start..($lines.Count - 1)]
    }

    $utf8NoBom = New-Object System.Text.UTF8Encoding $false
    [System.IO.File]::WriteAllLines($DestinationPath, $filtered, $utf8NoBom)
}

$browserPath = Find-BrowserPath
if ($browserPath) {
    Write-Host "Using browser: $browserPath"
    $env:PUPPETEER_EXECUTABLE_PATH = $browserPath
    $env:PUPPETEER_SKIP_DOWNLOAD = "true"
}
else {
    Write-Host "No local Chrome/Edge found. Installing Chromium for Puppeteer ..."
    & npx --yes puppeteer browsers install chrome
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to install Chromium for Puppeteer."
    }
}

$config = Join-Path $repoRoot "docs\pdf-config.js"
if (-not (Test-Path $config)) {
    throw "Missing config: $config"
}

# Keep temporary Markdown in the repo root so relative image paths
# such as docs/ScreenShot.png resolve the same way as in the READMEs.
$jobs = @(
    @{
        Source = "README.md"
        TempMd = ".pdf-src-README.md"
        Generated = ".pdf-src-README.pdf"
        Dest = "docs\README.pdf"
        Title = "MIDI Hold Repairer"
    },
    @{
        Source = "README-ja.md"
        TempMd = ".pdf-src-README-ja.md"
        Generated = ".pdf-src-README-ja.pdf"
        Dest = "docs\README-ja.pdf"
        Title = "MIDI Hold Repairer"
    }
)

foreach ($job in $jobs) {
    if (-not (Test-Path $job.Source)) {
        throw "Missing source: $($job.Source)"
    }

    $tempMd = Join-Path $repoRoot $job.TempMd
    New-PdfSourceMarkdown -SourcePath (Join-Path $repoRoot $job.Source) -DestinationPath $tempMd

    Write-Host "Building $($job.Dest) from $($job.Source) ..."
    try {
        & npx --yes "md-to-pdf@5.2.4" `
            $tempMd `
            --basedir $repoRoot `
            --config-file $config `
            --document-title $job.Title `
            --page-media-type print

        if ($LASTEXITCODE -ne 0) {
            throw "Failed to build $($job.Source)"
        }
        if (-not (Test-Path $job.Generated)) {
            throw "PDF was not created: $($job.Generated)"
        }

        $destPath = Join-Path $repoRoot $job.Dest
        try {
            [System.IO.File]::Copy((Resolve-Path $job.Generated), $destPath, $true)
        }
        catch {
            $fallback = "$destPath.new"
            [System.IO.File]::Copy((Resolve-Path $job.Generated), $fallback, $true)
            Write-Warning "Could not overwrite $destPath (file may be open). Wrote $fallback instead."
        }
    }
    finally {
        Remove-Item -Force -Path $tempMd -ErrorAction SilentlyContinue
        Remove-Item -Force -Path $job.Generated -ErrorAction SilentlyContinue
    }
}

Remove-Item -Recurse -Force -Path (Join-Path $repoRoot ".pdf-build") -ErrorAction SilentlyContinue

Write-Host "Done."
Get-ChildItem docs\README*.pdf | Select-Object Name, Length, LastWriteTime
