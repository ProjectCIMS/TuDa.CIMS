#!/usr/bin/env pwsh
#requires -Version 5

param(
  [Parameter(Position = 0)]
  [string]$Workdir,

  [Parameter(ValueFromRemainingArguments = $true)]
  [string[]]$DockerArgs,

  [switch]$Local,
  [switch]$Rebuild,
  [string]$Image
)

$ErrorActionPreference = 'Stop'

if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
  Write-Error "Docker is not installed or not on PATH."
}

# Resolve repo root based on this script's location so paths work regardless of the caller's CWD
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$RepoRoot = (Resolve-Path (Join-Path $ScriptDir '..\..')).Path

# Default to registry image to match docker-compose; allow override via param or env.
$DefaultRegistryImage = 'ghcr.io/projectcims/cims-asset-importer:latest'
$Image = if ($PSBoundParameters.ContainsKey('Image')) { $Image } elseif ($env:IMAGE) { $env:IMAGE } else { $DefaultRegistryImage }

# Default Dockerfile/Context anchored at repo root; allow env overrides
$Dockerfile = if ($env:DOCKERFILE) { $env:DOCKERFILE } else { Join-Path $RepoRoot 'utils\TuDa.CIMS.AssetItemImporter\Dockerfile' }
if (-not [System.IO.Path]::IsPathRooted($Dockerfile)) { $Dockerfile = (Resolve-Path (Join-Path (Get-Location) $Dockerfile)).Path } else { $Dockerfile = (Resolve-Path $Dockerfile).Path }

$Context = if ($env:CONTEXT) { $env:CONTEXT } else { $RepoRoot }
if (-not [System.IO.Path]::IsPathRooted($Context)) { $Context = (Resolve-Path (Join-Path (Get-Location) $Context)).Path } else { $Context = (Resolve-Path $Context).Path }

# If Workdir not passed positionally, try to consume the first remaining DockerArgs as Workdir for convenience
if (-not $Workdir -and $DockerArgs -and $DockerArgs.Length -gt 0) {
  $Workdir = $DockerArgs[0]
  $DockerArgs = $DockerArgs[1..($DockerArgs.Length-1)]
}

if (-not $Workdir) { throw "Usage: run-container-attach.ps1 [-Local] [-Rebuild] [-Image <tag>] <host-workdir> [extra docker args]" }
if (-not (Test-Path -LiteralPath $Workdir)) { throw "Workdir does not exist: $Workdir" }

$FullPath = [System.IO.Path]::GetFullPath($Workdir)
$mount = ($FullPath + ':/work')

# Local build path: only build if -Local is specified
if ($Local) {
  if (-not $PSBoundParameters.ContainsKey('Image') -and -not $env:IMAGE -and $Image -eq $DefaultRegistryImage) {
    # If user didn't provide an image explicitly, use a sensible local default
    $Image = 'cims-asset-importer:local'
  }

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
  }
} elseif ($Rebuild -or ($env:REBUILD -eq '1')) {
  Write-Warning "-Rebuild ignored because -Local not set (using registry image)"
}

# Build full argument list explicitly to avoid parser quirks
$runArgs = @('run','--rm','-it','--entrypoint','sh','-v',$mount,'-w','/work')
if ($DockerArgs) { $runArgs += $DockerArgs }
$runArgs += @($Image)

& docker @runArgs

