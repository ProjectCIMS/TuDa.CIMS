#!/usr/bin/env bash
set -euo pipefail

# Starts the AssetItemImporter container with a provided work directory and attaches a shell.
# Usage: ./utils/TuDa.CIMS.AssetItemImporter/run-container-attach.sh <host-workdir> [extra docker args]

IMAGE="${IMAGE:-cims-asset-importer:local}"
DOCKERFILE="${DOCKERFILE:-utils/TuDa.CIMS.AssetItemImporter/Dockerfile}"
CONTEXT="${CONTEXT:-.}"

if [[ $# -lt 1 ]]; then
  echo "Usage: $0 <host-workdir> [extra docker args]" >&2
  exit 1
fi

WORKDIR="$1"; shift || true

# Build the image if it doesn't exist locally.
if ! docker image inspect "$IMAGE" >/dev/null 2>&1; then
  echo "Image '$IMAGE' not found locally. Building..."
  docker build -t "$IMAGE" -f "$DOCKERFILE" "$CONTEXT"
fi

# Attach an interactive shell with the workdir mounted.
exec docker run --rm -it --entrypoint sh \
  -v "${WORKDIR}:/work" -w /work \
  "$IMAGE" "$@"

