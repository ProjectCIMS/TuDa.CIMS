using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.AssetItemImporter.Reader;

public class ConsumableReader(string path) : AssetItemReader(path)
{
    private const int Name = 1;
    private const int Shop = 2;
    private const int ItemNumber = 3;
    private const int Price = 4;
    private const int Room = 5;
    private const int Amount = 6;
    private const int SerialNumber = 7;
    private const int Manufacturer = 8;

    public override IEnumerable<CreateAssetItemDto> GetAssetItems()
    {
        var worksheet = Workbook.Worksheets.First();
        return worksheet
            .RowsUsed()
            .Skip(1)
            .Select(row => new CreateConsumableDto
            {
                Name = row.Cell(Name).GetString(),
                Shop = row.Cell(Shop).GetString(),
                ItemNumber = row.Cell(ItemNumber).GetString(),
                Price = row.Cell(Price).GetDouble(),
                Room = Enum.Parse<Rooms>(row.Cell(Room).GetString()),
                Amount = row.Cell(Amount).TryGetValue(out int amount) ? amount : 0,
                SerialNumber = row.Cell(SerialNumber).GetString(),
                Manufacturer = row.Cell(Manufacturer).GetString(),
                // Not in the Excel
                Note = "",
                ExcludeFromConsumableStatistics = true, // Initial one
            });
    }
}
