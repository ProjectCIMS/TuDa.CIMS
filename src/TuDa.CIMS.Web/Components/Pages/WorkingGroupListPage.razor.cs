using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Components.WorkingGroupList;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class WorkingGroupListPage
{
    [Inject] private IDialogService DialogService { get; set; } = null!;

    [Inject] private IWorkingGroupApi _workingGroupApi { get; set; } = null!;

    private WorkingGroupPageWorkingGroupList _workingGroupList = null!;

    private async Task OpenDialogAsync()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        var dialog = await DialogService.ShowAsync<WorkingGroupPageWorkingGroupListAddDialog>(
            "AddWorkingGroupDialog",
            options
        );
        var result = await dialog.Result;
        var professor = await dialog.GetReturnValueAsync<CreateProfessorDto>();

        if (result is { Canceled: false })
        {
            if (professor != null)
            {
                await _workingGroupApi.CreateAsync(
                    new CreateWorkingGroupDto { Professor = professor }
                );
                await _workingGroupList.ReloadDataGridAsync();
            }
        }
    }
}
