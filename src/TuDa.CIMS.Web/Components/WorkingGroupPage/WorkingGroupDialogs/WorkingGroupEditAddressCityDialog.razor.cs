using Microsoft.AspNetCore.Components;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage.WorkingGroupDialogs;

public partial class WorkingGroupEditAddressCityDialog : ComponentBase
{
    [Parameter] public string ProfessorCity { get; set; }

    public void Save()
    {
        // Save the city
    }

    public void Cancel()
    {
        // Cancel the dialog
    }
}

