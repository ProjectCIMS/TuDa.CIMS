using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Test.Faker;

public class SolventFaker : SubstanceFaker<Solvent>
{
    public SolventFaker(Room? room = null, List<Hazard>? hazards = null)
        : base(room, hazards)
    {
        RuleFor(s => s.BindingSize, f => f.Random.Double(max: 40));
    }
}
