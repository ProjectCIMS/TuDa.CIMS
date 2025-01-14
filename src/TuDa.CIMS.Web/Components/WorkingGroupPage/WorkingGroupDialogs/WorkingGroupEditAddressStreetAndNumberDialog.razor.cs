using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage.WorkingGroupDialogs;

public partial class WorkingGroupEditAddressStreetAndNumberDialog(IWorkingGroupApi _workingGroupApi) : ComponentBase
{
    [Parameter] public string ProfessorStreet { get; set; }
    [Parameter] public int ProfessorNumber { get; set; }
    [Parameter] public Guid WorkingGroupId { get; set; }


    [CascadingParameter] public required MudDialogInstance MudDialog { get; set; }


    public async Task Save()
    {
        // Saves the street and number

        MudDialog.Close(DialogResult.Ok(ProfessorStreet));
    }

    public void Cancel()
    {
        // Cancel the dialog

        MudDialog.Cancel();
    }
}

