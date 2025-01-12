using Microsoft.AspNetCore.Components;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage.WorkingGroupDialogs;

public partial class WorkingGroupEditPhoneNumberDialog : ComponentBase
{
    [Parameter] public string ProfessorPhoneNumber { get; set; }

    public void Save()
    {
        // Save the city
    }

    public void Cancel()
    {
        // Cancel the dialog
    }
}

