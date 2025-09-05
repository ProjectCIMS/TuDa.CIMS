#!/usr/bin/env sh
set -e

# Pass-through wrapper for the AssetItemImporter CLI
exec dotnet TuDa.CIMS.AssetItemImporter.dll "$@"

