using TuDa.CIMS.AssetItemImporter.Extensions;
using TuDa.CIMS.Shared.Dtos.Create;

namespace TuDa.CIMS.AssetItemImporter.Reader;

/// <summary>
/// Reader for chemical items where the binding size and unit are provided in a single column.
/// Produces <see cref="TuDa.CIMS.Shared.Dtos.Create.CreateChemicalDto"/> instances.
/// </summary>
public class ChemicalReader(string path) : AssetItemReader(path)
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
        public const int BindingSizeAndUnit = 8;

        /// <summary>
        /// Not used, as <see cref="BindingSizeAndUnit"/> has unit inside.
        /// </summary>
        public const int Unit = 9;
    }

    public override IEnumerable<CreateAssetItemDto> GetAssetItems() =>
        Workbook
            .Worksheets.First()
            .RowsUsed()
            .Skip(1)
            .Select(row =>
            {
                var (bindingSize, priceUnit) = GetBindingSize(
                    row.Cell(Indexes.BindingSizeAndUnit).GetString()
                );

                return new CreateChemicalDto
                {
                    Name = row.Cell(Indexes.Name).GetString(),
                    Shop = row.Cell(Indexes.Shop).GetString(),
                    ItemNumber = row.Cell(Indexes.ItemNumber).GetString(),
                    Price = row.Cell(Indexes.Price).GetDoubleOrDefault(),
                    Room = GetRoom(row.Cell(Indexes.Room).GetString()),
                    Cas = row.Cell(Indexes.Cas).GetString(),
                    Purity = row.Cell(Indexes.Purity).GetString(),
                    BindingSize = bindingSize,
                    PriceUnit = priceUnit,
                    // Not in the Excel
                    Note = string.Empty,
                };
            });
}
