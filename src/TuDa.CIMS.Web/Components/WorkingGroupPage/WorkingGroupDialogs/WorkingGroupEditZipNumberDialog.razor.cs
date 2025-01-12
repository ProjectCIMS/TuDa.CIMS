using Microsoft.AspNetCore.Components;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage.WorkingGroupDialogs;

public partial class WorkingGroupEditZipNumberDialog : ComponentBase
{
    [Parameter] public string ProfessorZipNumber { get; set; }

    public void Save()
    {
        // Save the city
    }

    public void Cancel()
    {
        // Cancel the dialog
    }
}

