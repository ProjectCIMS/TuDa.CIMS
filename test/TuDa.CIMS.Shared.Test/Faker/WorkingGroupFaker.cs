using Bogus;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Test.Faker;

public class WorkingGroupFaker : BaseEntityFaker<WorkingGroup>
{
    public WorkingGroupFaker(
        Professor? professor = null,
        List<Student>? students = null,
        List<Purchase>? purchases = null
    )
    {
        RuleFor(g => g.Professor, () => professor ?? new PersonFaker<Professor>());
        RuleFor(
            g => g.Students,
            () => students ?? new PersonFaker<Student>().GenerateBetween(3, 5)
        );
        RuleFor(g => g.PhoneNumber, f => f.Phone.PhoneNumber());
        RuleFor(g => g.Email, f => f.Internet.Email());
        RuleFor(
            g => g.Purchases,
            (_, wg) => purchases ?? new PurchaseFaker(wg).GenerateBetween(3, 5)
        );
    }
}
