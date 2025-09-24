#!/usr/bin/env bash

# Ensure the script is run with Bash, not sh/dash
if [ -z "${BASH_VERSION:-}" ]; then
  echo "This script requires Bash. Run as './run-container-attach.sh' or 'bash run-container-attach.sh' (not 'sh')." >&2
  exit 1
fi

set -euo pipefail

# Starts the AssetItemImporter container with a provided work directory and attaches a shell.
# Usage: ./utils/TuDa.CIMS.AssetItemImporter/run-container-attach.sh <host-workdir> [--local|--rebuild] [extra docker args]

# Default to GitHub registry image, allow override via environment
IMAGE="${IMAGE:-ghcr.io/projectcims/cims-asset-item-importer:latest}"
DOCKERFILE="${DOCKERFILE:-utils/TuDa.CIMS.AssetItemImporter/Dockerfile}"
CONTEXT="${CONTEXT:-.}"
REBUILD_FLAG="${REBUILD:-0}"
LOCAL_BUILD="${LOCAL_BUILD:-0}"

if [[ $# -lt 1 ]]; then
  echo "Usage: $0 <host-workdir> [--local|--rebuild] [extra docker args]" >&2
  echo "  --local   : Build and use local container instead of pulling from registry" >&2
  echo "  --rebuild : Force rebuild of local container (implies --local)" >&2
  exit 1
fi

WORKDIR="$1"; shift || true

# Parse flags
while [[ $# -gt 0 ]]; do
  case "${1:-}" in
    --local)
      LOCAL_BUILD=1
      IMAGE="cims-asset-importer:local"
      shift
      ;;
    --rebuild)
      REBUILD_FLAG=1
      LOCAL_BUILD=1
      IMAGE="cims-asset-importer:local"
      shift
      ;;
    *)
      break
      ;;
  esac
done

# Handle container image: pull from registry or build locally
if [[ "$LOCAL_BUILD" == "1" ]]; then
  # Build local image if requested
  if ! docker image inspect "$IMAGE" >/dev/null 2>&1 || [[ "$REBUILD_FLAG" == "1" ]]; then
    if [[ "$REBUILD_FLAG" == "1" ]]; then
      echo "Rebuilding local image '$IMAGE' ..."
      docker build --no-cache -t "$IMAGE" -f "$DOCKERFILE" "$CONTEXT"
    else
      echo "Local image '$IMAGE' not found. Building..."
      docker build -t "$IMAGE" -f "$DOCKERFILE" "$CONTEXT"
    fi
  else
    echo "Using existing local image '$IMAGE'"
  fi
else
  # Pull from GitHub registry
  if ! docker image inspect "$IMAGE" >/dev/null 2>&1; then
    echo "Pulling latest image from GitHub registry: $IMAGE"
    docker pull "$IMAGE"
  else
    echo "Checking for updates to registry image: $IMAGE"
    docker pull "$IMAGE" || echo "Warning: Failed to pull updates, using existing local image"
  fi
fi

# Attach an interactive shell with the workdir mounted.
exec docker run --rm -it --entrypoint sh \
  -v "${WORKDIR}:/work" -w /work \
  "$IMAGE" "$@"
