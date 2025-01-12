using Microsoft.AspNetCore.Components;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage.WorkingGroupDialogs;

public partial class WorkingGroupEditAddressStreetAndNumberDialog : ComponentBase
{
    [Parameter] public string ProfessorStreet { get; set; }
    [Parameter] public string ProfessorNumber { get; set; }

    public void Save()
    {
        // Save the city
    }

    public void Cancel()
    {
        // Cancel the dialog
    }
}

