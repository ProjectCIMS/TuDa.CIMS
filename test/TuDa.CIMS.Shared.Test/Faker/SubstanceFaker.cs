using Bogus;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Test.Faker;

public abstract class SubstanceFaker<TSubstance> : AssetItemFaker<TSubstance>
    where TSubstance : Substance
{
    protected SubstanceFaker(Room? room = null, List<Hazard>? hazards = null)
        : base(room)
    {
        RuleFor(c => c.Cas, f => f.Random.ReplaceNumbers("###-#-#"));
        RuleFor(c => c.Purity, f => f.Random.Double());
        RuleFor(c => c.PriceUnit, f => f.Random.Enum<PriceUnits>());
        RuleFor(c => c.Hazards, () => hazards ?? new HazardFaker().GenerateBetween(1, 5));
    }
}
