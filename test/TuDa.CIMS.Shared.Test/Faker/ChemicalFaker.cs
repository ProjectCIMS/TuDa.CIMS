using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Test.Faker;

public class ChemicalFaker : SubstanceFaker<Chemical>
{
    public ChemicalFaker(Room? room = null, List<Hazard>? hazards = null)
        : base(room, hazards)
    {
        RuleFor(c => c.BindingSize, f => f.Random.Double(max: 20));
    }
}
