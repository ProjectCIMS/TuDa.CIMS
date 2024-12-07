using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Test.Faker;

public abstract class AssetItemFaker<TAssetItem> : BaseEntityFaker<TAssetItem>
    where TAssetItem : AssetItem
{
    protected AssetItemFaker(Room? room)
    {
        RuleFor(c => c.Price, f => f.Random.Double(max: 100));
        RuleFor(c => c.Room, () => room ?? new RoomFaker());
        RuleFor(c => c.Name, f => f.Commerce.ProductMaterial());
        RuleFor(c => c.ItemNumber, f => f.Commerce.Ean8());
        RuleFor(c => c.Shop, f => f.Company.CompanyName());
        RuleFor(c => c.Note, f => f.Lorem.Sentence());
    }
}
