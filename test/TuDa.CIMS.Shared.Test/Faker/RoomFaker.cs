using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Test.Faker;

public class RoomFaker : BaseEntityFaker<Room>
{
    public RoomFaker()
    {
        RuleFor(r => r.Name, f => f.Address.City());
    }
}
