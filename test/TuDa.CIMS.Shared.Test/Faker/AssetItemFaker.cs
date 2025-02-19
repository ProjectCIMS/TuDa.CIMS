using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Test.Faker;

public abstract class AssetItemFaker<TAssetItem> : BaseEntityFaker<TAssetItem>
    where TAssetItem : AssetItem
{
    protected AssetItemFaker(Rooms? room)
    {
        RuleFor(a => a.Price, f => f.Random.Double(max: 100));
        RuleFor(a => a.Room, f => room ?? f.Random.Enum<Rooms>());
        RuleFor(a => a.Name, f => f.Commerce.ProductMaterial());
        RuleFor(a => a.ItemNumber, f => f.Commerce.Ean8());
        RuleFor(a => a.Shop, f => f.Company.CompanyName());
        RuleFor(a => a.Note, f => f.Lorem.Sentence());
    }
}
