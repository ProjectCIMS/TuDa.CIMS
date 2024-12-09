using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Test.Faker;

public class HazardFaker : BaseEntityFaker<Hazard>
{
    public HazardFaker()
    {
        RuleFor(h => h.Name, f => f.Commerce.ProductAdjective());
        RuleFor(h => h.ImagePath, f => f.Image.PicsumUrl());
    }
}
