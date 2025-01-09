using Bogus;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Test.Faker;

public class BaseEntityFaker<TEntity> : Faker<TEntity>
    where TEntity : BaseEntity
{
    /// <summary>
    /// Static switch to enable creation of random Guids for Entities.
    /// Only set on True in UnitTests!
    /// </summary>
    public static bool UseRandomGuid = false;

    public BaseEntityFaker()
    {
        if (UseRandomGuid)
        {
            RuleFor(c => c.Id, f => f.Random.Guid());
        }
    }
}
