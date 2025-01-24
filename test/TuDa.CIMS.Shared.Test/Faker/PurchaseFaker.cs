using Bogus;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Test.Faker;

public class PurchaseFaker : BaseEntityFaker<Purchase>
{
    public PurchaseFaker(
        WorkingGroup workingGroup,
        List<PurchaseEntry>? purchaseEntries = null,
        bool? completed = null
    )
    {
        completed ??= new Randomizer().Bool();

        RuleFor(p => p.Buyer, f => f.PickRandom(workingGroup.Students));
        RuleFor(p => p.Entries, f => purchaseEntries ?? RandomPurchaseEntries());
        RuleFor(
            p => p.CompletionDate,
            f =>
                completed.Value
                    ? f.Date.RecentDateOnly(30).ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)
                :  null
        );
    }

    private static List<PurchaseEntry> RandomPurchaseEntries() =>
        new List<PurchaseEntry>()
            .Concat(new PurchaseEntryFaker<Chemical>(new ChemicalFaker()).GenerateBetween(0, 5))
            .Concat(new PurchaseEntryFaker<Consumable>(new ConsumableFaker()).GenerateBetween(0, 5))
            .Concat(new PurchaseEntryFaker<Solvent>(new SolventFaker()).GenerateBetween(0, 5))
            .Concat(
                new PurchaseEntryFaker<GasCylinder>(new GasCylinderFaker()).GenerateBetween(0, 5)
            )
            .ToList();
}
