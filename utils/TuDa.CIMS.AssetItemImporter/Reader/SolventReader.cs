using TuDa.CIMS.AssetItemImporter.Extensions;
using TuDa.CIMS.Shared.Dtos.Create;

namespace TuDa.CIMS.AssetItemImporter.Reader;

/// <summary>
/// Reader for solvent items; unit is embedded with binding size.
/// Produces <see cref="TuDa.CIMS.Shared.Dtos.Create.CreateSolventDto"/> instances.
/// </summary>
public class SolventReader(string path) : AssetItemReader(path)
{
    private static class Indexes
    {
        public const int Name = 1;
        public const int Shop = 2;
        public const int ItemNumber = 3;
        public const int Price = 4;
        public const int Room = 5;
        public const int BindingSizeAndUnit = 6;

        /// <summary>
        /// Not used, as <see cref="BindingSizeAndUnit"/> has unit inside.
        /// </summary>
        public const int Unit = 7;
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

                return new CreateSolventDto
                {
                    Name = row.Cell(Indexes.Name).GetString(),
                    Shop = row.Cell(Indexes.Shop).GetString(),
                    ItemNumber = row.Cell(Indexes.ItemNumber).GetString(),
                    Price = row.Cell(Indexes.Price).GetDoubleOrDefault(),
                    Room = GetRoom(row.Cell(Indexes.Room).GetString()),
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
