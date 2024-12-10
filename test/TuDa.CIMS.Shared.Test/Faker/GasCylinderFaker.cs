using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Test.Faker;

public class GasCylinderFaker : SubstanceFaker<GasCylinder>
{
    public GasCylinderFaker(Room? room = null, List<Hazard>? hazards = null)
        : base(room, hazards)
    {
        RuleFor(g => g.Volume, f => f.Random.Double(max: 300));
        RuleFor(g => g.Pressure, f => f.Random.Double(max: 35));
    }
}
