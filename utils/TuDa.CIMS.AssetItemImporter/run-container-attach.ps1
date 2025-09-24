#!/usr/bin/env pwsh
#requires -Version 5

param(
  [Parameter(Mandatory = $true, Position = 0)]
  [string]$Workdir,

  [Parameter(ValueFromRemainingArguments = $true)]
  [string[]]$DockerArgs,

  [switch]$Rebuild,
  [switch]$Local
)

$ErrorActionPreference = 'Stop'

if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
  Write-Error "Docker is not installed or not on PATH."
}

# Resolve repo root based on this script's location so paths work regardless of the caller's CWD
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$RepoRoot = (Resolve-Path (Join-Path $ScriptDir '..\..')).Path

# Default to GitHub registry image, allow override via environment or flag
$Image = if ($env:IMAGE) { 
  $env:IMAGE 
} elseif ($Local -or $Rebuild) { 
  'cims-asset-importer:local' 
} else { 
  'ghcr.io/projectcims/cims-asset-item-importer:latest' 
}

# Default Dockerfile/Context anchored at repo root; allow env overrides
$Dockerfile = if ($env:DOCKERFILE) { $env:DOCKERFILE } else { Join-Path $RepoRoot 'utils\TuDa.CIMS.AssetItemImporter\Dockerfile' }
if (-not [System.IO.Path]::IsPathRooted($Dockerfile)) { $Dockerfile = (Resolve-Path (Join-Path (Get-Location) $Dockerfile)).Path } else { $Dockerfile = (Resolve-Path $Dockerfile).Path }

$Context = if ($env:CONTEXT) { $env:CONTEXT } else { $RepoRoot }
if (-not [System.IO.Path]::IsPathRooted($Context)) { $Context = (Resolve-Path (Join-Path (Get-Location) $Context)).Path } else { $Context = (Resolve-Path $Context).Path }

if (-not (Test-Path -LiteralPath $Workdir)) {
  Write-Error "Workdir does not exist: $Workdir"
}

$FullPath = [System.IO.Path]::GetFullPath($Workdir)
$mount = ($FullPath + ':/work')

# Handle container image: pull from registry or build locally
if ($Local -or $Rebuild) {
  # Build local image if requested
  Write-Host "Using Dockerfile: $Dockerfile"
  Write-Host "Using context:     $Context"
  
  & docker image inspect $Image *> $null
  if ($LASTEXITCODE -ne 0) {
    Write-Host "Local image '$Image' not found. Building..."
    & docker build -t $Image -f $Dockerfile $Context
    if ($LASTEXITCODE -ne 0) {
      throw "Docker build failed. See output above. Dockerfile='$Dockerfile' Context='$Context'"
    }
  } elseif ($Rebuild -or ($env:REBUILD -eq '1')) {
    Write-Host "Rebuilding local image '$Image' ..."
    & docker build --no-cache -t $Image -f $Dockerfile $Context
    if ($LASTEXITCODE -ne 0) {
      throw "Docker rebuild failed. See output above. Dockerfile='$Dockerfile' Context='$Context'"
    }
  } else {
    Write-Host "Using existing local image '$Image'"
  }
} else {
  # Pull from GitHub registry
  & docker image inspect $Image *> $null
  if ($LASTEXITCODE -ne 0) {
    Write-Host "Pulling latest image from GitHub registry: $Image"
    & docker pull $Image
    if ($LASTEXITCODE -ne 0) {
      throw "Docker pull failed for image: $Image"
    }
  } else {
    Write-Host "Checking for updates to registry image: $Image"
    & docker pull $Image
    if ($LASTEXITCODE -ne 0) {
      Write-Warning "Failed to pull updates, using existing local image"
    }
  }
}

# Build full argument list explicitly to avoid parser quirks
$runArgs = @('run','--rm','-it','--entrypoint','sh','-v',$mount,'-w','/work')
if ($DockerArgs) { $runArgs += $DockerArgs }
$runArgs += @($Image)

& docker @runArgs

