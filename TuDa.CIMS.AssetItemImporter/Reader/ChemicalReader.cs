using TuDa.CIMS.Shared.Dtos.Create;

namespace TuDa.CIMS.AssetItemImporter.Reader;

public class ChemicalReader(string path) : AssetItemReader(path)
{
    private const int Name = 1;
    private const int Shop = 2;
    private const int ItemNumber = 3;
    private const int Price = 4;
    private const int Room = 5;
    private const int Cas = 6;
    private const int Purity = 7;
    private const int BindingSizeAndUnit = 8;

    /// <summary>
    /// Not used, as <see cref="BindingSizeAndUnit"/> has unit inside.
    /// </summary>
    private const int Unit = 9;

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

                return new CreateChemicalDto
                {
                    Name = row.Cell(Name).GetString(),
                    Shop = row.Cell(Shop).GetString(),
                    ItemNumber = row.Cell(ItemNumber).GetString(),
                    Price = row.Cell(Price).GetDouble(),
                    Room = GetRoom(row.Cell(Room).GetString()),
                    Cas = row.Cell(Cas).GetString(),
                    Purity = row.Cell(Purity).GetString(),
                    BindingSize = bindingSize,
                    PriceUnit = priceUnit,
                    // Not in the Excel
                    Note = string.Empty,
                };
            });
}
