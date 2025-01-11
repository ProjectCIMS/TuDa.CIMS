using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;
public partial class WorkingGroupHeader : ComponentBase
{
    private readonly IWorkingGroupApi _workingGroupApi;

    [Parameter] public required Professor Professor { get; set; }

    [Parameter] public string ProfessorName { get; set; } = String.Empty;

    public Guid workingGroupId { get; set; }

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private Guid GetIdFromUrl()
    {
        var uri = NavigationManager.Uri.Split('/');
        var idString = uri[^1];
        return Guid.Parse(idString);
    }

    protected override async Task OnInitializedAsync()
    {
        var workingGroup = await _workingGroupApi.GetAsync(workingGroupId);
        ProfessorName = workingGroup.Value.Professor.Name;
        Professor = workingGroup.Value.Professor;

        await base.OnInitializedAsync();
    }

    public void GetWorkingGroupId()
    {
        workingGroupId = GetIdFromUrl();
    }

    /// TODO: Add dialog
    public void OpenDialogAsync()
    {

    }
}
