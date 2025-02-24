using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Test.Faker;

public class GasCylinderFaker : SubstanceFaker<GasCylinder>
{
    public GasCylinderFaker(Rooms? room = null, List<Hazard>? hazards = null)
        : base(room, hazards)
    {
        RuleFor(g => g.Volume, f => f.Random.Double(max: 300));
        RuleFor(g => g.Pressure, f => f.Random.Double(max: 35));
    }
}
