using System.ComponentModel;
using ClosedXML.Excel;
using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.AssetItemImporter.Reader;

/// <summary>
/// Base class for reading asset items from an Excel workbook using ClosedXML
/// (<see cref="ClosedXML.Excel.XLWorkbook"/>). Concrete readers interpret
/// specific column layouts per <see cref="AssetItemType"/>.
/// </summary>
public abstract class AssetItemReader
{
    protected readonly XLWorkbook Workbook;
    protected const string Unknown = "Unknown";

    protected AssetItemReader(XLWorkbook workbook) => Workbook = workbook;

    protected AssetItemReader(string path) => Workbook = new XLWorkbook(path);

    /// <summary>
    /// Factory that returns a concrete reader for the given <see cref="AssetItemType"/>.
    /// </summary>
    public static AssetItemReader FromAssetItemType(
        AssetItemType assetItemType,
        string excelPath
    ) =>
        assetItemType switch
        {
            AssetItemType.Lösungsmittel => new SolventReader(excelPath),
            AssetItemType.Laborgeräte => new ConsumableReader(excelPath),
            AssetItemType.Gase => new GasReader(excelPath),
            AssetItemType.Chemikalien => new ChemicalReader(excelPath),
            _ => throw new InvalidEnumArgumentException(
                $"Unknown asset item type: {assetItemType}"
            ),
        };

    /// <summary>
    /// Read and transform rows from the workbook into DTOs ready for API submission
    /// (<see cref="TuDa.CIMS.Shared.Dtos.Create.CreateAssetItemDto"/>).
    /// </summary>
    public abstract IEnumerable<CreateAssetItemDto> GetAssetItems();

    /// <summary>Parse a <see cref="Rooms"/> value, or <see cref="Rooms.None"/> if unknown.</summary>
    protected static Rooms GetRoom(string roomString) =>
        Enum.TryParse(roomString, out Rooms room) ? room : Rooms.None;

    /// <summary>
    /// Parses binding size and unit from a value like "500 ml" or "500".
    /// Returns the numeric size and a <see cref="MeasurementUnits"/> value.
    /// </summary>
    protected static (double BindingSize, MeasurementUnits PriceUnit) GetBindingSize(string input)
    {
        string[] splitted = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        double bindingSize;
        MeasurementUnits priceUnit;
        if (splitted.Length == 2)
        {
            bindingSize = double.TryParse(splitted[0], out double result) ? result : 0;
            priceUnit = ParsePriceUnit(splitted[1]);
        }
        else
        {
            if (double.TryParse(splitted[0], out double result))
            {
                bindingSize = result;
                priceUnit = MeasurementUnits.Piece;
            }
            else
            {
                bindingSize = 0;
                priceUnit = ParsePriceUnit(splitted[0]);
            }
        }

        return (bindingSize, priceUnit);
    }

    /// <summary>Map unit string tokens to <see cref="MeasurementUnits"/>.</summary>
    private static MeasurementUnits ParsePriceUnit(string input) =>
        input switch
        {
            "ml" => MeasurementUnits.MilliLiter,
            "l" => MeasurementUnits.Liter,
            "g" => MeasurementUnits.Gram,
            "kg" => MeasurementUnits.KiloGram,
            _ => MeasurementUnits.Piece,
        };
}
