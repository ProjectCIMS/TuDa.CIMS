#!/usr/bin/env sh
set -e

# Pass-through wrapper for the AssetItemImporter CLI
exec dotnet /app/TuDa.CIMS.AssetItemImporter.dll "$@"
