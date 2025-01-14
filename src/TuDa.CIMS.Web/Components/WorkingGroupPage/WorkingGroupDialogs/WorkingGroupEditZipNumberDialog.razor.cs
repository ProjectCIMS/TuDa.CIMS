using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage.WorkingGroupDialogs;

public partial class WorkingGroupEditZipNumberDialog : ComponentBase
{
    [Parameter] public string ProfessorZipNumber { get; set; }
    [CascadingParameter] public required MudDialogInstance MudDialog { get; set; }

    public async Task Save()
    {
        // Saves the zip code

        MudDialog.Close(DialogResult.Ok(ProfessorZipNumber));
    }

    public void Cancel()
    {
        // Cancel the dialog

        MudDialog.Cancel();
    }
}

