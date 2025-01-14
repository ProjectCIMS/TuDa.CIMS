using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage.WorkingGroupDialogs;

public partial class WorkingGroupProfessorEditDialog(IWorkingGroupApi _workingGroupApi) : ComponentBase
{
    [CascadingParameter] public required MudDialogInstance MudDialog { get; set; }

    [Parameter] public Guid WorkingGroupId { get; set; }

    [Parameter] public WorkingGroup? currentWorkingGroup { get; set; }

    [Parameter] public required string ProfessorName { get; set; } = String.Empty;

    private async Task Save()
    {


        MudDialog.Close(DialogResult.Ok(ProfessorName));
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
