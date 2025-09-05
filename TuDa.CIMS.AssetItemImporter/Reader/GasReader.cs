using TuDa.CIMS.AssetItemImporter.Extensions;
using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.AssetItemImporter.Reader;

public class GasReader(string path) : AssetItemReader(path)
{
    private const int Name = 1;
    private const int Shop = 2;
    private const int ItemNumber = 3;
    private const int Price = 4;
    private const int Room = 5;
    private const int Cas = 6;
    private const int Purity = 7;
    private const int Volume = 9;
    private const int Pressure = 10;
    private const int Producer = 11;

    public override IEnumerable<CreateAssetItemDto> GetAssetItems() =>
        Workbook
            .Worksheets.First()
            .RowsUsed()
            .Skip(1)
            .Select(row => new CreateGasCylinderDto
            {
                Name = row.Cell(Name).GetString(),
                Shop = row.Cell(Shop).GetString(),
                ItemNumber = row.Cell(ItemNumber).GetString(),
                Price = row.Cell(Price).GetDoubleOrDefault(),
                Room = GetRoom(row.Cell(Room).GetString()),
                Cas = row.Cell(Cas).GetString(),
                Purity = row.Cell(Purity).GetString(),
                PriceUnit = MeasurementUnits.Piece,
                Volume = ParseDoubleFromAnyString(row.Cell(Volume).GetString()),
                Pressure = ParseDoubleFromAnyString(row.Cell(Pressure).GetString()),
                Note = $"Producer {row.Cell(Producer).GetString()}",
                // Not in the Excel
                Hazards = [],
            });

    private static double ParseDoubleFromAnyString(string volume) =>
        double.Parse(string.Concat(volume.TakeWhile(x => x is (>= '0' and <= '9') or '.')));
}
