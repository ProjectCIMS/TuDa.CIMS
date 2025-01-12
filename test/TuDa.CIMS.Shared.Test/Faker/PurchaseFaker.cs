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
        RuleFor(
            p => p.Entries,
            () => purchaseEntries ?? new PurchaseEntryFaker().GenerateBetween(2, 10)
        );
        RuleFor(
            p => p.CompletionDate,
            f =>
                completed.Value
                    ? f.Date.RecentDateOnly(30).ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)
                    : null
        );
        RuleFor(p => p.Completed, () => completed);
    }
}
