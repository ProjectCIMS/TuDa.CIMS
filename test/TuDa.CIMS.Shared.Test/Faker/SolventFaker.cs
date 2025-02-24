using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Test.Faker;

public class SolventFaker : SubstanceFaker<Solvent>
{
    public SolventFaker(Rooms? room = null, List<Hazard>? hazards = null)
        : base(room, hazards)
    {
        RuleFor(s => s.BindingSize, f => f.Random.Double(max: 40));
    }
}
