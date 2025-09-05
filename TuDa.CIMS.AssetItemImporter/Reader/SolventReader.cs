using TuDa.CIMS.AssetItemImporter.Extensions;
using TuDa.CIMS.Shared.Dtos.Create;

namespace TuDa.CIMS.AssetItemImporter.Reader;

public class SolventReader(string path) : AssetItemReader(path)
{
    private const int Name = 1;
    private const int Shop = 2;
    private const int ItemNumber = 3;
    private const int Price = 4;
    private const int Room = 5;
    private const int BindingSizeAndUnit = 6;

    /// <summary>
    /// Not used, as <see cref="BindingSizeAndUnit"/> has unit inside.
    /// </summary>
    private const int Unit = 7;

    public override IEnumerable<CreateAssetItemDto> GetAssetItems() =>
        Workbook
            .Worksheets.First()
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
                    Price = row.Cell(Price).GetDoubleOrDefault(),
                    Room = GetRoom(row.Cell(Room).GetString()),
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
