using Microsoft.AspNetCore.Components;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage.WorkingGroupDialogs;

public partial class WorkingGroupEditEmailDialog : ComponentBase
{
    [Parameter] public string ProfessorEmailAddress { get; set; }

    public void Save()
    {
        // Save the city
    }

    public void Cancel()
    {
        // Cancel the dialog
    }
}

