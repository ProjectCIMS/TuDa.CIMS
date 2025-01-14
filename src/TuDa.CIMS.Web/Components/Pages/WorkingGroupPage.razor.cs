using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Components.Dashboard;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class WorkingGroupPage
{
    [Inject]
    private IDialogService DialogService { get; set; } = null!;
    private async Task OpenDialogAsync()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        var dialog = await DialogService.ShowAsync<WorkingGroupPageWorkingGroupListAddDialog>(
            "AddWorkingGroupDialog",options
        );
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            Professor professor = (Professor)result.Data!;
            if (professor.Name != null)
            {
                //AddWorkingGroup(professor);
            }
        }
    }
}
