# AssetItemImporter

## Overview

- Purpose: CLI to parse Excel lists of lab assets (chemicals, solvents, gases, consumables) and either preview or import them into TuDa CIMS.
- Commands: `check` (preview) and `import` (create via API).

## Requirements

- **.NET SDK:** `net9.0`
- **Excel:** Data in the expected column order (see below).

## Commands

- Help: `dotnet run -- --help`
- Check: `dotnet run -- check <type> "<path-to.xlsx>"`
  - Shows a summary of parsed items without calling the API.
  - Arguments:
    - `<type>`: Asset item type (see Types below).
    - `<path-to.xlsx>`: Path to the Excel file.
- Import: `dotnet run -- import <type> "<path-to.xlsx>" <api-url>`
  - Parses the Excel file and sends items to the TuDa CIMS API.
  - Arguments:
    - `<type>`: Asset item type (see Types below).
    - `<path-to.xlsx>`: Path to the Excel file.
    - `<api-url>`: TuDa CIMS API base URL, e.g., `https://host`.

### Container usage

The AssetItemImporter is available as a prebuild container from the GitHub registry, eliminating the need to build locally:

- **Use prebuild container (recommended):**
  - Attach shell: `./utils/TuDa.CIMS.AssetItemImporter/run-container-attach.sh /your/host/dir`
  - PowerShell: `./utils/TuDa.CIMS.AssetItemImporter/run-container-attach.ps1 /your/host/dir`

- **Build and use local container (for development/testing):**
  - Attach shell: `./utils/TuDa.CIMS.AssetItemImporter/run-container-attach.sh /your/host/dir --local`
  - PowerShell: `./utils/TuDa.CIMS.AssetItemImporter/run-container-attach.ps1 /your/host/dir -Local`
  - Force rebuild: Add `--rebuild` (shell) or `-Rebuild` (PowerShell) flag

- **Inside container, run:**
  - Check: `asset-importer check <type> "/work/file.xlsx"`
  - Import: `asset-importer import <type> "/work/file.xlsx" <api-url>`

**Container image:** `ghcr.io/projectcims/cims-asset-item-importer:latest`

## Types

- Current enum values (case-insensitive):
  - `chemikalien` (Chemicals)
  - `lösungsmittel` (Solvents)
  - `gase` (Gases)
  - `laborgeräte` (Consumables/Lab equipment)
- If your shell shows garbled umlauts, ensure UTF-8 encoding or use PowerShell 7.

## Excel Columns

- **Chemicals:** Name, Shop, ItemNumber, Price, Room, CAS, Purity, BindingSize+Unit (e.g., `500 ml`). Unit column (if present) is ignored.
- **Solvents:** Name, Shop, ItemNumber, Price, Room, BindingSize+Unit. Unit column is ignored.
- **Gases:** Name, Shop, ItemNumber, Price, Room, CAS, Purity, Volume, Pressure, Producer.
- **Consumables:** Name, Shop, ItemNumber, Price, Room, Amount, SerialNumber, Manufacturer.

## Development Notes

- Entry point: `Program.cs` wires commands using System.CommandLine.
- Commands: Implemented with `CommandBase` and `CommandArgument<T>` in `Commands/`.
- Readers: Implementations in `Reader/*Reader.cs` (all inherit `AssetItemReader`).
- Utilities: Excel helpers in `Extensions/IXLCellExtension.cs`.
