using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage.WorkingGroupDialogs;

public partial class WorkingGroupEditAddressCityDialog(IWorkingGroupApi _workingGroupApi) : ComponentBase
{
    [Parameter] public string ProfessorCity { get; set; }

    [Parameter] public Guid WorkingGroupId { get; set; }

    [CascadingParameter] public required MudDialogInstance MudDialog { get; set; }


    public async Task Save()
    {
        // Saves the city

        MudDialog.Close(DialogResult.Ok(ProfessorCity));
    }

    public void Cancel()
    {
        // Cancel the dialog
        MudDialog.Cancel();
    }
}

