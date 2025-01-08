using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Test.Faker;

public class PersonFaker<TPerson> : BaseEntityFaker<TPerson>
    where TPerson : Person
{
    public PersonFaker()
    {
        RuleFor(p => p.Name, f => f.Name.LastName());
        RuleFor(p => p.FirstName, f => f.Name.FirstName());
        RuleFor(p => p.Gender, f => f.PickRandom<Gender>());
    }
}
