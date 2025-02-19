using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Test.Faker;

public class ChemicalFaker : SubstanceFaker<Chemical>
{
    public ChemicalFaker(Rooms? room = null, List<Hazard>? hazards = null)
        : base(room, hazards)
    {
        RuleFor(c => c.BindingSize, f => f.Random.Double(max: 20));
    }
}
