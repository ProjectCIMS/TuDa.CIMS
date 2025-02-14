using Bogus;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Test.Faker;

public class PurchaseEntryFaker<T, TAssetItem> : BaseEntityFaker<T>
    where T : PurchaseEntry
    where TAssetItem : AssetItem
{
    public PurchaseEntryFaker(TAssetItem? assetItem)
        : this(null, assetItem) { }

    public PurchaseEntryFaker(
        AssetItemFaker<TAssetItem>? assetItemFaker = null,
        TAssetItem? assetItem = null
    )
    {
        RuleFor(
            e => e.AssetItem,
            f =>
                assetItemFaker
                ?? assetItem
                ?? f.PickRandom(
                    new List<AssetItem>
                    {
                        new ChemicalFaker(),
                        new ConsumableFaker(),
                        new SolventFaker(),
                        new GasCylinderFaker(),
                    }
                )
        );
        RuleFor(e => e.Amount, f => f.Random.Double(min: 1, max: 100));
        RuleFor(e => e.PricePerItem, f => f.Random.Double(max: 1000));
    }
}

public class PurchaseEntryFaker<TAssetItem>(
    AssetItemFaker<TAssetItem>? assetItemFaker = null,
    TAssetItem? assetItem = null
) : PurchaseEntryFaker<PurchaseEntry, TAssetItem>(assetItemFaker, assetItem)
    where TAssetItem : AssetItem
{
    public PurchaseEntryFaker(TAssetItem assetItem)
        : this(null, assetItem) { }
}
