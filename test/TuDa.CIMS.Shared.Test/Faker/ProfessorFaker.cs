using Bogus;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Test.Faker;

public class ProfessorFaker : PersonFaker<Professor>
{
    public ProfessorFaker()
    {
        RuleFor(p => p.Address, () => new AddressFaker());
    }
}
