using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public partial class WorkingGroupInformation : ComponentBase
{

    /// Probewerte für Professor
    private Professor ProfessorInfoa = new Professor
    {
        FirstName = "Max",
        Name = "",
        Address = new Address() { Street = "Musterstraße", Number = 1, ZipCode = "12345", City = "Musterstadt" },
        Email = "sigma@sigma.de"
    };
    public void Edit(){}
}

