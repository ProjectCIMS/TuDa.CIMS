using TuDa.CIMS.AssetItemImporter.Extensions;
using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.AssetItemImporter.Reader;

/// <summary>
/// Reader for gas cylinder items with volume and pressure columns.
/// Produces <see cref="TuDa.CIMS.Shared.Dtos.Create.CreateGasCylinderDto"/> instances.
/// </summary>
public class GasReader(string path) : AssetItemReader(path)
{
    private static class Indexes
    {
        public const int Name = 1;
        public const int Shop = 2;
        public const int ItemNumber = 3;
        public const int Price = 4;
        public const int Room = 5;
        public const int Cas = 6;
        public const int Purity = 7;
        public const int Volume = 9;
        public const int Pressure = 10;
        public const int Producer = 11;
    }

    public override IEnumerable<CreateAssetItemDto> GetAssetItems() =>
        Workbook
            .Worksheets.First()
            .RowsUsed()
            .Skip(1)
            .Select(row => new CreateGasCylinderDto
            {
                Name = row.Cell(Indexes.Name).GetString(),
                Shop = row.Cell(Indexes.Shop).GetString(),
                ItemNumber = row.Cell(Indexes.ItemNumber).GetString(),
                Price = row.Cell(Indexes.Price).GetDoubleOrDefault(),
                Room = GetRoom(row.Cell(Indexes.Room).GetString()),
                Cas = row.Cell(Indexes.Cas).GetString(),
                Purity = row.Cell(Indexes.Purity).GetString(),
                PriceUnit = MeasurementUnits.Piece,
                Volume = ParseDoubleFromAnyString(row.Cell(Indexes.Volume).GetString()),
                Pressure = ParseDoubleFromAnyString(row.Cell(Indexes.Pressure).GetString()),
                Note = $"Producer {row.Cell(Indexes.Producer).GetString()}",
                // Not in the Excel
                Hazards = [],
            });

    /// <summary>
    /// Extract a numeric prefix (including decimal point) from a string and parse as <see cref="double"/>.
    /// </summary>
    private static double ParseDoubleFromAnyString(string volume)
    {
        string numericString = string.Concat(
            volume.TakeWhile(x => x is (>= '0' and <= '9') or '.')
        );
        return double.TryParse(numericString, out double result) ? result : 0.0;
    }
}
