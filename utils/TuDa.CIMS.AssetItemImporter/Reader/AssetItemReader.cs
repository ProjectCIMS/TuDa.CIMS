using System.ComponentModel;
using ClosedXML.Excel;
using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.AssetItemImporter.Reader;

public abstract class AssetItemReader
{
    protected readonly XLWorkbook Workbook;
    protected const string Unknown = "Unknown";

    protected AssetItemReader(XLWorkbook workbook) => Workbook = workbook;

    protected AssetItemReader(string path) => Workbook = new XLWorkbook(path);

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

    public abstract IEnumerable<CreateAssetItemDto> GetAssetItems();

    protected static Rooms GetRoom(string roomString) =>
        Enum.TryParse(roomString, out Rooms room) ? room : Rooms.None;

    protected static (double BindingSize, MeasurementUnits PriceUnit) GetBindingSize(string input)
    {
        string[] splitted = input.Split(" ");
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
