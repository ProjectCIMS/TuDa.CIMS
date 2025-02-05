using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public partial class WorkingGroupHeader(IWorkingGroupApi workingGroupApi, NavigationManager navigation) : ComponentBase
{
    [Parameter] public required Professor Professor { get; set; }

    [Parameter] public string ProfessorName { get; set; } = String.Empty;

    [Parameter] public string ProfessorTitle { get; set; } = String.Empty;

    [Parameter] public Guid WorkingGroupId { get; set; }

    [Inject] private IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// Sets the ProfessorName and Professor properties
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        var workingGroup = await workingGroupApi.GetAsync(WorkingGroupId);
        ProfessorName = workingGroup.Value.Professor.Name;
        ProfessorTitle = workingGroup.Value.Professor.Title;
        Professor = workingGroup.Value.Professor;

        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Opens the dialog to look up and edit the workinggroup.
    /// </summary>
    public async void OpenInformationDialog()
    {
        var dialogReference = DialogService.Show<WorkingGroupInfoPopOut>("Informationen zur Arbeitsgruppe",
            new DialogParameters { { "WorkingGroupId", WorkingGroupId } });

        // Wait for the dialog to close and then reload the page
        await dialogReference.Result;
        navigation.NavigateTo(navigation.Uri, forceLoad: true);
    }
}
