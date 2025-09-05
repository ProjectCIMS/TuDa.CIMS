using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.AssetItemImporter.Reader;

public class SolventReader(string path) : AssetItemReader(path)
{
    private const int Name = 1;
    private const int Shop = 2;
    private const int ItemNumber = 3;
    private const int Price = 4;
    private const int Room = 5;
    private const int BindingSizeAndUnit = 6;

    public override IEnumerable<CreateAssetItemDto> GetAssetItems()
    {
        var worksheet = Workbook.Worksheets.First();
        return worksheet
            .RowsUsed()
            .Skip(1)
            .Select(row =>
            {
                var (bindingSize, priceUnit) = GetBindingSize(
                    row.Cell(BindingSizeAndUnit).GetString()
                );

                return new CreateSolventDto
                {
                    Name = row.Cell(Name).GetString(),
                    Shop = row.Cell(Shop).GetString(),
                    ItemNumber = row.Cell(ItemNumber).GetString(),
                    Price = row.Cell(Price).GetDouble(),
                    Room = Enum.Parse<Rooms>(row.Cell(Room).GetString()),
                    PriceUnit = priceUnit,
                    BindingSize = bindingSize,
                    // Not in the Excel
                    Hazards = [],
                    Note = string.Empty,
                    Cas = Unknown,
                    Purity = Unknown,
                };
            });
    }

    private static (double BindingSize, MeasurementUnits PriceUnit) GetBindingSize(string input)
    {
        string[] splitted = input.Split(" ");
        double bindingSize = double.TryParse(splitted[0], out double result) ? result : 0;
        MeasurementUnits priceUnit =
            splitted.Length >= 2 ? ParsePriceUnit(splitted[1]) : MeasurementUnits.Piece;

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
