using ClosedXML.Excel;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.ExcelImporter;

public class SubstanceExcelReader : CimsExcelReader
{
    public SubstanceExcelReader() : base(ExcelFilePath("Substances.xlsx"))
    {
    }

    public IEnumerable<Solvent> ReadSolvents()
    {
        var worksheet = Workbook.Worksheet("Lösungsmittel");
        var rows = worksheet.RowsUsed().Skip(1);

        foreach (IXLRow row in rows)
        {
            yield return new Solvent
            {
                Name = row.Cell(1)
                    .GetString(),
                Purity = row.Cell(2)
                    .GetString(),
                ItemNumber = row.Cell(3)
                    .GetString(),
                Shop = row.Cell(4)
                    .GetString(),
                BindingSize = Double.Parse(row.Cell(5)
                    .GetString()
                    .Trim('l')),
                Price = Double.Parse(row.Cell(6)
                    .GetString()),
                PriceUnit = MeasurementUnits.Liter,
                Room = StubRoom,
                Cas = StubCas,
            };
        }
    }

    public IEnumerable<GasCylinder> ReadGasCylinders()
    {
        var worksheet = Workbook.Worksheet("Gase");
        var rows = worksheet.RowsUsed().Skip(2);

        foreach (IXLRow row in rows)
        {
            yield return new GasCylinder
            {
                Name = row.Cell(1).GetString(),
                Purity = row.Cell(2).GetString(),
                Pressure = Int32.Parse(row.Cell(3).GetString().Replace("bar", "")),
                Volume = Int32.Parse(row.Cell(4).GetString().Replace("Liter", "")),
                Price = row.Cell(4).GetDouble(),
                PriceUnit = MeasurementUnits.Piece,
                Room = StubRoom,
                Cas = StubCas,
            };
        }
    }

    public IEnumerable<Chemical> ReadChemicals()
    {
        var worksheet = Workbook.Worksheet("Chemikalien");
        var rows = worksheet.RowsUsed().Skip(1);

        foreach (IXLRow row in rows)
        {
            var binding = row.Cell(5).GetString()!;
            MeasurementUnits priceUnit;
            Double bindingSize;

            if (IsGram(binding, out bindingSize)) priceUnit = MeasurementUnits.Gram;
            else if (IsKiloGram(binding, out bindingSize)) priceUnit = MeasurementUnits.KiloGram;
            else if (IsLiter(binding, out bindingSize)) priceUnit = MeasurementUnits.Liter;
            else
            {
                priceUnit = MeasurementUnits.Piece;
                bindingSize = Double.Parse(binding);
            }

            yield return new Chemical()
            {
                Name = row.Cell(1).GetString(),
                Purity = row.Cell(2).GetString(),
                ItemNumber = row.Cell(3).GetString(),
                Shop = row.Cell(4).GetString(),
                BindingSize = bindingSize,
                PriceUnit = priceUnit,
                Price = Double.Parse(row.Cell(6).GetString().Replace(".-", "")),
                Room = StubRoom,
                Cas = StubCas,
            };
        }
    }

    private static bool IsGram(string input, out double amount)
        => Double.TryParse(input.Replace("g", ""), out amount);

    private static bool IsKiloGram(string input, out double amount)
        => Double.TryParse(input.Replace("kg", ""), out amount);

    private static bool IsLiter(string input, out double amount)
        => Double.TryParse(input.Replace("l", ""), out amount);
}
