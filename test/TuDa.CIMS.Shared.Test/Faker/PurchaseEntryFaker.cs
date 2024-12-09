using Bogus;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Test.Faker;

public class PurchaseEntryFaker : BaseEntityFaker<PurchaseEntry>
{
    public PurchaseEntryFaker(AssetItem? assetItem = null)
    {
        RuleFor(
            e => e.AssetItem,
            f =>
                assetItem
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
        RuleFor(e => e.Amount, f => f.Random.Int(min: 1, max: 40));
        RuleFor(e => e.PricePerItem, f => f.Random.Double(max: 50));
    }
}
