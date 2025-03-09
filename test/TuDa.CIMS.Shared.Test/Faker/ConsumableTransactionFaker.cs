using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Test.Faker;

public class ConsumableTransactionFaker : BaseEntityFaker<ConsumableTransaction>
{
    public ConsumableTransactionFaker(Consumable? consumable = null)
    {
        RuleFor(t => t.Consumable, _ => consumable ?? new ConsumableFaker().Generate());
        RuleFor(t => t.Date, f => f.Date.Recent().ToUniversalTime());
        RuleFor(t => t.AmountChange, f => f.Random.Int(-100, 100));
        RuleFor(
            t => t.TransactionReason,
            f => f.Random.Enum<TransactionReasons>(TransactionReasons.Init)
        );
    }
}
