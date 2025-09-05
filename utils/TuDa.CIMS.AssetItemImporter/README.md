# AssetItemImporter

## Overview

- **Purpose:** CLI to parse Excel lists of lab assets (chemicals, solvents, gases, consumables) and either preview or import them into TuDa CIMS.
- **Status:** Two commands available: `check` (preview) and `import` (create via API).

## Requirements

- **.NET SDK:** `net9.0`
- **Excel:** Data in the expected column order (see below).

## Commands

- **Help:** `dotnet run -- --help`
- **Check:** `dotnet run -- check <type> "<path-to.xlsx>"`
  - Shows a summary of parsed items without calling the API.
  - Arguments:
    - `<type>`: Asset item type (see Types below).
    - `<path-to.xlsx>`: Path to the Excel file.
- **Import:** `dotnet run -- import <type> "<path-to.xlsx>" <api-url>`
  - Parses the Excel file and sends items to the TuDa CIMS API.
  - Arguments:
    - `<type>`: Asset item type (see Types below).
    - `<path-to.xlsx>`: Path to the Excel file.
    - `<api-url>`: TuDa CIMS API base URL, e.g., `https://host`.

## Types

- Current enum values (case-insensitive):
  - `chemikalien` (Chemicals)
  - `l�sungsmittel` (Solvents)
  - `gase` (Gases)
  - `laborger�te` (Consumables/Lab equipment)
- If your shell shows garbled umlauts, ensure UTF-8 encoding or use PowerShell 7.

## Excel Columns

- **Chemicals:** Name, Shop, ItemNumber, Price, Room, CAS, Purity, BindingSize+Unit (e.g., `500 ml`). Unit column (if present) is ignored.
- **Solvents:** Name, Shop, ItemNumber, Price, Room, BindingSize+Unit. Unit column is ignored.
- **Gases:** Name, Shop, ItemNumber, Price, Room, CAS, Purity, Volume, Pressure, Producer.
- **Consumables:** Name, Shop, ItemNumber, Price, Room, Amount, SerialNumber, Manufacturer.

## Development Notes

- **Entry point:** `Program.cs` with Cocona commands `check` and `import`.
- **Readers:** Implementations in `Reader/*Reader.cs` (all inherit `AssetItemReader`).
- **Utilities:** Excel helpers in `Extensions/IXLCellExtension.cs`.
