using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Test.Faker;

public class PersonFaker<TPerson> : BaseEntityFaker<TPerson>
    where TPerson : Person
{
    public PersonFaker(WorkingGroup? workingGroup = null)
    {
        RuleFor(p => p.Name, f => f.Name.LastName());
        RuleFor(p => p.FirstName, f => f.Name.FirstName());
        RuleFor(p => p.WorkingGroup, () => workingGroup ?? new WorkingGroupFaker().Generate());
    }
}
