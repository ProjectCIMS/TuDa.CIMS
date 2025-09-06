#!/usr/bin/env bash

# Ensure the script is run with Bash, not sh/dash
if [ -z "${BASH_VERSION:-}" ]; then
  echo "This script requires Bash. Run as './run-container-attach.sh' or 'bash run-container-attach.sh' (not 'sh')." >&2
  exit 1
fi

set -euo pipefail

# Starts the AssetItemImporter container with a provided work directory and attaches a shell.
# Usage: ./utils/TuDa.CIMS.AssetItemImporter/run-container-attach.sh <host-workdir> [extra docker args]

IMAGE="${IMAGE:-cims-asset-importer:local}"
DOCKERFILE="${DOCKERFILE:-utils/TuDa.CIMS.AssetItemImporter/Dockerfile}"
CONTEXT="${CONTEXT:-.}"
REBUILD_FLAG="${REBUILD:-0}"

if [[ $# -lt 1 ]]; then
  echo "Usage: $0 <host-workdir> [extra docker args]" >&2
  exit 1
fi

WORKDIR="$1"; shift || true

# Optional --rebuild flag triggers a force build
if [[ "${1:-}" == "--rebuild" ]]; then
  REBUILD_FLAG=1
  shift
fi

# Build the image if it doesn't exist locally, or if rebuild requested.
if ! docker image inspect "$IMAGE" >/dev/null 2>&1; then
  echo "Image '$IMAGE' not found locally. Building..."
  docker build -t "$IMAGE" -f "$DOCKERFILE" "$CONTEXT"
elif [[ "$REBUILD_FLAG" == "1" ]]; then
  echo "Rebuilding image '$IMAGE' ..."
  docker build --no-cache -t "$IMAGE" -f "$DOCKERFILE" "$CONTEXT"
fi

# Attach an interactive shell with the workdir mounted.
exec docker run --rm -it --entrypoint sh \
  -v "${WORKDIR}:/work" -w /work \
  "$IMAGE" "$@"
