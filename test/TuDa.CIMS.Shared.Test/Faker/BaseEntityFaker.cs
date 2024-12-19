using Bogus;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Test.Faker;

public class BaseEntityFaker<TEntity> : Faker<TEntity>
    where TEntity : BaseEntity
{
    public BaseEntityFaker()
    {
        RuleFor(c => c.Id, f => f.Random.Guid());
    }
}
