using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage.WorkingGroupDialogs;

public partial class WorkingGroupEditAddressStreetAndNumberDialog : ComponentBase
{
    [Parameter] public string? Street { get; set; }
    [Parameter] public int Number { get; set; }

    // TODO: include the number
    // [Parameter] public string Number { get; set; }


    [CascadingParameter] public required MudDialogInstance MudDialog { get; set; }


    public async Task Save()
    {
        // Saves the street and number

        MudDialog.Close(DialogResult.Ok(Street));
    }

    public void Cancel()
    {
        // Cancel the dialog

        MudDialog.Cancel();
    }
}

