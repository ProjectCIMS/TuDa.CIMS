#!/usr/bin/env pwsh
#requires -Version 5

param(
  [Parameter(Mandatory = $true, Position = 0)]
  [string]$Workdir,

  [Parameter(ValueFromRemainingArguments = $true)]
  [string[]]$DockerArgs
)

$ErrorActionPreference = 'Stop'

if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
  Write-Error "Docker is not installed or not on PATH."
}

# Resolve repo root based on this script's location so paths work regardless of the caller's CWD
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$RepoRoot = (Resolve-Path (Join-Path $ScriptDir '..\..')).Path

$Image = if ($env:IMAGE) { $env:IMAGE } else { 'cims-asset-importer:local' }

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

# Inspect image; if missing, build it
Write-Host "Using Dockerfile: $Dockerfile"
Write-Host "Using context:     $Context"

& docker image inspect $Image *> $null
if ($LASTEXITCODE -ne 0) {
  Write-Host "Image '$Image' not found locally. Building..."
  & docker build -t $Image -f $Dockerfile $Context
  if ($LASTEXITCODE -ne 0) {
    throw "Docker build failed. See output above. Dockerfile='$Dockerfile' Context='$Context'"
  }
}

# Build full argument list explicitly to avoid parser quirks
$runArgs = @('run','--rm','-it','--entrypoint','sh','-v',$mount,'-w','/work')
if ($DockerArgs) { $runArgs += $DockerArgs }
$runArgs += @($Image)

& docker @runArgs

