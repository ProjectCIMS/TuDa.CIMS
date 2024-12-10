using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Test.Faker;

public abstract class AssetItemFaker<TAssetItem> : BaseEntityFaker<TAssetItem>
    where TAssetItem : AssetItem
{
    protected AssetItemFaker(Room? room)
    {
        RuleFor(a => a.Price, f => f.Random.Double(max: 100));
        RuleFor(a => a.Room, () => room ?? new RoomFaker());
        RuleFor(a => a.Name, f => f.Commerce.ProductMaterial());
        RuleFor(a => a.ItemNumber, f => f.Commerce.Ean8());
        RuleFor(a => a.Shop, f => f.Company.CompanyName());
        RuleFor(a => a.Note, f => f.Lorem.Sentence());
    }
}
