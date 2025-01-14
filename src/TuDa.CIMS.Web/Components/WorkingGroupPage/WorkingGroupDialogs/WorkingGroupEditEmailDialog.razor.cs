using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage.WorkingGroupDialogs;

public partial class WorkingGroupEditEmailDialog : ComponentBase
{
    [Parameter] public string ProfessorEmailAddress { get; set; }

    [CascadingParameter] public required MudDialogInstance MudDialog { get; set; }


    public async Task Save()
    {
        // Saves the email

        MudDialog.Close(DialogResult.Ok(ProfessorEmailAddress));
    }

    public void Cancel()
    {
        // Cancel the dialog

        MudDialog.Cancel();
    }
}

