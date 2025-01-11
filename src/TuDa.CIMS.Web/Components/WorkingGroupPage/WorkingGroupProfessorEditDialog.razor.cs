using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public partial class WorkingGroupProfessorEditDialog : ComponentBase
{

    [CascadingParameter] public required MudDialogInstance MudDialog { get; set; }

    [Parameter] public Guid WorkingGroupId { get; set; }

    /// Current name of the Professor
    [Parameter]
    public string CurrentName { get; set; }

    /// New name of the Professor
    private string ProfessorName;

    /// TODO: Add API call to update the Professor
    private void Save()
    {
        MudDialog.Close(DialogResult.Ok(ProfessorName));
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
