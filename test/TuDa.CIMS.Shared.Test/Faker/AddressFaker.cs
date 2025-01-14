using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Test.Faker;

public class AddressFaker : BaseEntityFaker<Address>
{
    public AddressFaker()
    {
        RuleFor(a => a.Street, f => f.Address.StreetName());
        RuleFor(a => a.Number, f => f.Random.Int(1, 200));
        RuleFor(a => a.ZipCode, f => f.Address.ZipCode());
        RuleFor(a => a.City, f => f.Address.City());
    }
}
