#!/usr/bin/env bash

# Ensure the script is run with Bash, not sh/dash
if [ -z "${BASH_VERSION:-}" ]; then
  echo "This script requires Bash. Run as './run-container-attach.sh' or 'bash run-container-attach.sh' (not 'sh')." >&2
  exit 1
fi

set -euo pipefail

# Starts the AssetItemImporter container with a provided work directory and attaches a shell.
#
# Default: uses prebuilt registry image (same registry style as docker-compose).
# Local build: pass --local to use a local image tag and build if needed.
#
# Usage:
#   ./utils/TuDa.CIMS.AssetItemImporter/run-container-attach.sh [--local] [--rebuild] [--image <tag>] <host-workdir> [extra docker args]
#
# Env overrides:
#   IMAGE       - image tag to use (default: ghcr.io/projectcims/cims-asset-importer:latest)
#   DOCKERFILE  - Dockerfile to use when building locally
#   CONTEXT     - build context when building locally
#   REBUILD     - if "1", force rebuild on local build

set -euo pipefail

IMAGE_DEFAULT="ghcr.io/projectcims/cims-asset-importer:latest"
IMAGE="${IMAGE:-$IMAGE_DEFAULT}"
LOCAL_IMAGE_DEFAULT="cims-asset-importer:local"
DOCKERFILE="${DOCKERFILE:-utils/TuDa.CIMS.AssetItemImporter/Dockerfile}"
CONTEXT="${CONTEXT:-.}"
REBUILD_FLAG="${REBUILD:-0}"
USE_LOCAL=0
WORKDIR=""

print_usage() {
  echo "Usage: $0 [--local] [--rebuild] [--image <tag>] <host-workdir> [extra docker args]" >&2
}

# Parse flags and positional args
while [[ $# -gt 0 ]]; do
  case "$1" in
    --local)
      USE_LOCAL=1
      # If user didn't explicitly set IMAGE via env, use local default
      if [[ "${IMAGE:-}" == "$IMAGE_DEFAULT" ]]; then
        IMAGE="$LOCAL_IMAGE_DEFAULT"
      fi
      shift
      ;;
    --rebuild)
      REBUILD_FLAG=1
      shift
      ;;
    --image)
      [[ $# -ge 2 ]] || { echo "--image requires a value" >&2; exit 2; }
      IMAGE="$2"
      shift 2
      ;;
    -h|--help)
      print_usage
      exit 0
      ;;
    *)
      if [[ -z "$WORKDIR" ]]; then
        WORKDIR="$1"
        shift
      else
        break
      fi
      ;;
  esac
done

if [[ -z "$WORKDIR" ]]; then
  print_usage
  exit 1
fi

# If using local image, build if missing or if rebuild requested.
if [[ "$USE_LOCAL" == "1" ]]; then
  if ! docker image inspect "$IMAGE" >/dev/null 2>&1; then
    echo "Local image '$IMAGE' not found. Building..."
    docker build -t "$IMAGE" -f "$DOCKERFILE" "$CONTEXT"
  elif [[ "$REBUILD_FLAG" == "1" ]]; then
    echo "Rebuilding local image '$IMAGE' ..."
    docker build --no-cache -t "$IMAGE" -f "$DOCKERFILE" "$CONTEXT"
  fi
else
  if [[ "$REBUILD_FLAG" == "1" ]]; then
    echo "Warning: --rebuild ignored because --local not set (using registry image)" >&2
  fi
fi

# Attach an interactive shell with the workdir mounted.
exec docker run --rm -it --entrypoint sh \
  -v "${WORKDIR}:/work" -w /work \
  "$IMAGE" "$@"
