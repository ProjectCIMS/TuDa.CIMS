using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Test.Faker;

public class ConsumableFaker : AssetItemFaker<Consumable>
{
    public ConsumableFaker(Rooms? room = null)
        : base(room)
    {
        RuleFor(c => c.Manufacturer, f => f.Company.CompanyName());
        RuleFor(c => c.SerialNumber, f => f.Commerce.Ean13());
        RuleFor(c => c.Amount, f => f.Random.Int(1, 100));
    }
}
