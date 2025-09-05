using TuDa.CIMS.AssetItemImporter.Extensions;
using TuDa.CIMS.Shared.Dtos.Create;

namespace TuDa.CIMS.AssetItemImporter.Reader;

public class ConsumableReader(string path) : AssetItemReader(path)
{
    private static class Indexes
    {
        public const int Name = 1;
        public const int Shop = 2;
        public const int ItemNumber = 3;
        public const int Price = 4;
        public const int Room = 5;
        public const int Amount = 6;
        public const int SerialNumber = 7;
        public const int Manufacturer = 8;
    }

    public override IEnumerable<CreateAssetItemDto> GetAssetItems() =>
        Workbook
            .Worksheets.First()
            .RowsUsed()
            .Skip(1)
            .Select(row => new CreateConsumableDto
            {
                Name = row.Cell(Indexes.Name).GetString(),
                Shop = row.Cell(Indexes.Shop).GetString(),
                ItemNumber = row.Cell(Indexes.ItemNumber).GetString(),
                Price = row.Cell(Indexes.Price).GetDoubleOrDefault(),
                Room = GetRoom(row.Cell(Indexes.Room).GetString()),
                Amount = row.Cell(Indexes.Amount).GetIntOrDefault(),
                SerialNumber = row.Cell(Indexes.SerialNumber).GetString(),
                Manufacturer = row.Cell(Indexes.Manufacturer).GetString(),
                // Not in the Excel
                Note = string.Empty,
                ExcludeFromConsumableStatistics = true, // Initial one
            });
}
