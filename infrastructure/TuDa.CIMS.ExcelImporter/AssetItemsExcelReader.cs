using ClosedXML.Excel;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.ExcelImporter;

public class AssetItemsExcelReader(string path)
{
    private readonly XLWorkbook _workbook = new(path);
    private readonly Room _stubRoom = new() { Name = "None" };
    private const string StubCas = "Unknown";

    public IEnumerable<AssetItem> ReadAssetItems() =>
        new List<AssetItem>().Concat(ReadConsumables()).Concat(ReadSubstances());

    #region Consumables

    public IEnumerable<Consumable> ReadConsumables()
    {
        var worksheet = _workbook.Worksheet("Verbrauchsgegenstände");
        var rows = worksheet.RowsUsed();

        foreach (IXLRow row in rows)
        {
            if (ShouldBeSkipped(row))
                continue;

            yield return new Consumable
            {
                Name = row.Cell(3).GetString(),
                ItemNumber = row.Cell(10).GetString(),
                Price = Double.Parse(row.Cell(7).GetString()),
                Room = _stubRoom,
                Manufacturer = "Unknown",
                SerialNumber = "Unknown",
                Amount = int.TryParse(row.Cell(2).GetString(), out var amount) ? amount : -1,
            };
        }
    }

    private static bool ShouldBeSkipped(IXLRow row) =>
        string.IsNullOrWhiteSpace(row.Cell(1).GetString())
        || row.Cell(1).GetString().Contains("laufende")
        || row.Cell(1).GetString().Contains("Position");

    #endregion

    #region Substances

    public IEnumerable<Substance> ReadSubstances() =>
        new List<Substance>()
            .Concat(ReadSolvents())
            .Concat(ReadChemicals())
            .Concat(ReadGasCylinders());

    public IEnumerable<Solvent> ReadSolvents()
    {
        var worksheet = _workbook.Worksheet("Lösungsmittel");
        var rows = worksheet.RowsUsed().Skip(1);

        foreach (IXLRow row in rows)
        {
            yield return new Solvent
            {
                Name = row.Cell(1).GetString(),
                Purity = row.Cell(2).GetString(),
                ItemNumber = row.Cell(3).GetString(),
                Shop = row.Cell(4).GetString(),
                BindingSize = Double.Parse(row.Cell(5).GetString().Trim('l')),
                Price = Double.Parse(row.Cell(6).GetString()),
                PriceUnit = MeasurementUnits.Liter,
                Room = _stubRoom,
                Cas = StubCas,
            };
        }
    }

    public IEnumerable<GasCylinder> ReadGasCylinders()
    {
        var worksheet = _workbook.Worksheet("Gase");
        var rows = worksheet.RowsUsed().Skip(2);

        foreach (IXLRow row in rows)
        {
            yield return new GasCylinder
            {
                Name = row.Cell(1).GetString(),
                Purity = row.Cell(2).GetString(),
                Pressure = Int32.Parse(row.Cell(3).GetString().Replace("bar", "")),
                Volume = Int32.Parse(row.Cell(4).GetString().Replace("Liter", "")),
                Price = Double.Parse(row.Cell(5).GetString().Replace(" €", "")),
                PriceUnit = MeasurementUnits.Piece,
                Room = _stubRoom,
                Cas = StubCas,
            };
        }
    }

    public IEnumerable<Chemical> ReadChemicals()
    {
        var worksheet = _workbook.Worksheet("Chemikalien");
        var rows = worksheet.RowsUsed().Skip(1);

        foreach (IXLRow row in rows)
        {
            var binding = row.Cell(5).GetString()!;
            MeasurementUnits priceUnit;
            Double bindingSize;

            if (string.IsNullOrEmpty(binding))
            {
                priceUnit = MeasurementUnits.Piece;
                bindingSize = 0;
            }
            else if (IsGram(binding, out bindingSize))
                priceUnit = MeasurementUnits.Gram;
            else if (IsKiloGram(binding, out bindingSize))
                priceUnit = MeasurementUnits.KiloGram;
            else if (IsLiter(binding, out bindingSize))
                priceUnit = MeasurementUnits.Liter;
            else if (IsMilliLiter(binding, out bindingSize))
                priceUnit = MeasurementUnits.MilliLiter;
            else
            {
                priceUnit = MeasurementUnits.Piece;
                bindingSize = Double.Parse(binding.Split(' ')[0]);
            }

            yield return new Chemical()
            {
                Name = row.Cell(1).GetString(),
                Purity = row.Cell(2).GetString(),
                ItemNumber = row.Cell(3).GetString(),
                Shop = row.Cell(4).GetString(),
                BindingSize = bindingSize,
                PriceUnit = priceUnit,
                Price = Double.TryParse(row.Cell(6).GetString().Replace(".-", ""), out var price)
                    ? price
                    : 0,
                Room = _stubRoom,
                Cas = StubCas,
            };
        }
    }

    private static bool IsGram(string input, out double amount) =>
        Double.TryParse(input.Replace("g", ""), out amount);

    private static bool IsKiloGram(string input, out double amount) =>
        Double.TryParse(input.Replace("kg", ""), out amount);

    private static bool IsLiter(string input, out double amount) =>
        Double.TryParse(input.Replace("l", ""), out amount);

    private static bool IsMilliLiter(string input, out double amount) =>
        Double.TryParse(input.Replace("ml", ""), out amount);

    #endregion
}
