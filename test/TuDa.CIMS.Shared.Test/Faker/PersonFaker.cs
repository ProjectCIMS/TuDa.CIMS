using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Test.Faker;

public class PersonFaker<TPerson> : BaseEntityFaker<TPerson>
    where TPerson : Person
{
    public PersonFaker(WorkingGroup workingGroup)
    {
        RuleFor(p => p.Name, t => t.Name.LastName());
        RuleFor(p => p.FirstName, t => t.Name.FirstName());
        RuleFor(p => p.WorkingGroup, _ => workingGroup);
    }
}
