using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public partial class WorkingGroupHeader : ComponentBase
{
    [Parameter]
    public required WorkingGroupResponseDto WorkingGroup { get; set; }

    private Professor Professor => WorkingGroup.Professor;

    private readonly IDialogService _dialogService;
    private readonly IWorkingGroupApi _workingGroupApi;
    private readonly ISnackbar _snackbar;

    public WorkingGroupHeader(
        IDialogService dialogService,
        IWorkingGroupApi workingGroupApi,
        ISnackbar snackbar
    )
    {
        _dialogService = dialogService;
        _workingGroupApi = workingGroupApi;
        _snackbar = snackbar;
    }

    /// <summary>
    /// Opens the dialog to look up and edit the workinggroup.
    /// </summary>
    public async Task OpenInformationDialog()
    {
        var dialogReference = await _dialogService.ShowAsync<WorkingGroupInfoPopOut>(
            "Informationen zur Arbeitsgruppe",
            new DialogParameters<WorkingGroupInfoPopOut>
            {
                { pop => pop.WorkingGroup, WorkingGroup },
            }
        );

        await dialogReference.Result;
    }

    public async Task ToggleWorkingGroupStatus()
    {
        var success = await _workingGroupApi.ToggleActiveAsync(WorkingGroup.Id);
        if (success.IsError)
        {
            _snackbar.Add(
                "Etwas is schiefgelaufen die Arbeitsgruppe zu deaktivieren.",
                Severity.Error
            );
        }
        else
        {
            WorkingGroup.IsDeactivated = !WorkingGroup.IsDeactivated;
        }
    }
}
