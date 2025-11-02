Param(
    [string]$Configuration = "Release",
    [string]$Runtime = "win-x64",
    [switch]$SelfContained,
    [string]$Version = "",
    [string]$CliFolder = ""
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Write-Info($msg){ Write-Host "[INFO] $msg" -ForegroundColor Cyan }
function Write-Ok($msg){ Write-Host "[ OK ] $msg" -ForegroundColor Green }

# Resolve script directory robustly (handles dot-sourcing / null MyInvocation.MyCommand.Path)
$scriptDir = $null
if ($MyInvocation -and $MyInvocation.MyCommand -and -not [string]::IsNullOrWhiteSpace($MyInvocation.MyCommand.Path)) {
    $scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
} elseif (-not [string]::IsNullOrWhiteSpace($PSCommandPath)) {
    $scriptDir = Split-Path -Parent $PSCommandPath
} elseif (-not [string]::IsNullOrWhiteSpace($PSScriptRoot)) {
    # $PSScriptRoot is already a directory
    $scriptDir = $PSScriptRoot
}

if (-not $scriptDir) {
    Write-Error "Unable to determine script directory (MyInvocation.MyCommand.Path, PSCommandPath, and PSScriptRoot are empty)."
    exit 1
}
if (-not (Test-Path -LiteralPath $scriptDir)) {
    Write-Error "Resolved script directory does not exist: $scriptDir"
    exit 1
}

# Project root is parent of the scripts directory
try {
    $root = Split-Path -Parent $scriptDir
} catch {
    Write-Error "Failed to compute project root from '$scriptDir': $($_.Exception.Message)"
    exit 1
}
$proj = Join-Path $root "Barts Tok.csproj"
if (-not (Test-Path $proj)) { throw "Projektdatei nicht gefunden: $proj" }

# Resolve version
if (-not $Version) {
    # Try from csproj <Version> (search all PropertyGroup nodes)
    try {
        [xml]$csproj = Get-Content $proj -Raw -ErrorAction Stop
        # Use XPath to find any <Version> under any <PropertyGroup>
        $versionNodes = $csproj.SelectNodes('//Project/PropertyGroup/Version')
        if ($versionNodes -and $versionNodes.Count -gt 0) {
            foreach ($node in $versionNodes) {
                $val = [string]$node.InnerText
                if ($val -and $val.Trim()) { $Version = $val.Trim(); break }
            }
        }
        if (-not $Version) {
            Write-Warning "No <Version> element with a value found in '$proj'. Will try tag (GITHUB_REF_NAME) or fallback."
        }
    } catch {
        Write-Error "Failed to read/parse project file '$proj': $($_.Exception.Message)"
        exit 1
    }
}
if (-not $Version) {
    # Try from tag (CI)
    $ref = $env:GITHUB_REF_NAME
    if ($ref) { $Version = $ref.TrimStart('v') }
}
if (-not $Version) { $Version = "0.1.0" }

$dist = Join-Path $root "dist"
$outWpf = Join-Path $root "out/wpf/$Runtime"
if (Test-Path $dist) { Remove-Item $dist -Recurse -Force }
New-Item -ItemType Directory -Force -Path $dist | Out-Null
New-Item -ItemType Directory -Force -Path $outWpf | Out-Null

# Build / publish WPF
Write-Info "dotnet publish ($Configuration, $Runtime, SelfContained=$($SelfContained.IsPresent))"
$selfProp = if ($SelfContained) { "-p:SelfContained=true" } else { "-p:SelfContained=false" }
& dotnet publish $proj -c $Configuration -r $Runtime -p:PublishSingleFile=true -p:PublishTrimmed=false -p:DebugType=None -p:DebugSymbols=false $selfProp -o $outWpf

# Zip WPF output
$safeVersion = $Version -replace '[^0-9A-Za-z\.-]','-'
$zipWpfName = "BartsTOK-$safeVersion-windows-$Runtime.zip"
$zipWpfPath = Join-Path $dist $zipWpfName
if (Test-Path $zipWpfPath) { Remove-Item $zipWpfPath -Force }
Write-Info "Create zip: $zipWpfName"
Compress-Archive -Path (Join-Path $outWpf '*') -DestinationPath $zipWpfPath -CompressionLevel Optimal

# Optional: package CLI folder if provided
$zipCliPath = $null
if ($CliFolder -and (Test-Path $CliFolder)) {
    $absCli = Resolve-Path $CliFolder
    $zipCliName = "BartsTOK-CLI-$safeVersion-windows-$Runtime.zip"
    $zipCliPath = Join-Path $dist $zipCliName
    if (Test-Path $zipCliPath) { Remove-Item $zipCliPath -Force }
    Write-Info "Create zip (CLI): $zipCliName"
    Compress-Archive -Path (Join-Path $absCli '*') -DestinationPath $zipCliPath -CompressionLevel Optimal
}

# Checksums
function New-ChecksumFiles($filePath){
    $sha256 = Get-FileHash -Algorithm SHA256 -Path $filePath | Select-Object -ExpandProperty Hash
    $sha512 = Get-FileHash -Algorithm SHA512 -Path $filePath | Select-Object -ExpandProperty Hash
    $b = Split-Path -Leaf $filePath
    $sha256Line = "SHA256 ($b) = $sha256"
    $sha512Line = "SHA512 ($b) = $sha512"
    Set-Content -Path ("$filePath.sha256") -Value $sha256Line -Encoding ASCII
    Set-Content -Path ("$filePath.sha512") -Value $sha512Line -Encoding ASCII
    return @($sha256Line, $sha512Line)
}

$allLines = @()
$allLines += New-ChecksumFiles $zipWpfPath
if ($zipCliPath) { $allLines += New-ChecksumFiles $zipCliPath }
Set-Content -Path (Join-Path $dist "checksums.txt") -Value $allLines -Encoding ASCII

# dist/README.txt
$readme = @()
$readme += "BartsTOK Release Artifacts"
$readme += "================================"
$readme += ""
$readme += "Version: $Version"
$readme += "Runtime: $Runtime"
$readme += "Configuration: $Configuration"
$readme += "Self-contained: $($SelfContained.IsPresent)"
$readme += "Generated: $(Get-Date -Format 'u')"
$readme += ""
$readme += "Files:"
$readme += " - $(Split-Path -Leaf $zipWpfPath)"
if ($zipCliPath) { $readme += " - $(Split-Path -Leaf $zipCliPath)" }
$readme += " - checksums.txt"
$readme += " - *.sha256 / *.sha512"
$readme += ""
$readme += "Verify checksums (PowerShell):"
$readme += "  Get-FileHash -Algorithm SHA256 .\\$(Split-Path -Leaf $zipWpfPath)"
if ($zipCliPath) { $readme += "  Get-FileHash -Algorithm SHA256 .\\$(Split-Path -Leaf $zipCliPath)" }
$readme += ""
$readme += "Notes:"
$readme += " - WPF zip enthält die Anwendung (Barts Tok.exe und Abhängigkeiten)"
$readme += " - CLI zip (optional) enthält Kommandozeilen-Hilfen/Tools (falls angegeben)"
$readme += " - Siehe CHANGELOG.md und README.md im Repository für Details"
Set-Content -Path (Join-Path $dist "README.txt") -Value $readme -Encoding UTF8

Write-Ok "Artifacts in: $dist"
Get-ChildItem $dist | ForEach-Object { Write-Host " - " $_.Name }
