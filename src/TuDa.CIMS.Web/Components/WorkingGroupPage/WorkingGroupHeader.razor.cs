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
    private readonly NavigationManager _navigation;

    public WorkingGroupHeader(
        IDialogService dialogService,
        IWorkingGroupApi workingGroupApi,
        NavigationManager navigation
    )
    {
        _dialogService = dialogService;
        _workingGroupApi = workingGroupApi;
        _navigation = navigation;
    }

    /// <summary>
    /// Opens the dialog to look up and edit the workinggroup.
    /// </summary>
    public async Task OpenInformationDialog()
    {
        var dialogReference = _dialogService.Show<WorkingGroupInfoPopOut>(
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
        await _workingGroupApi.ToggleActiveAsync(WorkingGroup.Id);
    }
}
