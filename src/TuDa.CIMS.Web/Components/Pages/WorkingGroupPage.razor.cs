using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class WorkingGroupPage : ComponentBase
{
    [Parameter]
    public Guid WorkingGroupId { get; set; }

    public WorkingGroupResponseDto? WorkingGroup { get; set; }

    public readonly IWorkingGroupApi _workingGroupApi;

    public WorkingGroupPage(IWorkingGroupApi workingGroupApi) => _workingGroupApi = workingGroupApi;

    protected override async Task OnInitializedAsync()
    {
        var workingGroup = await _workingGroupApi.GetAsync(WorkingGroupId);
        if (!workingGroup.IsError)
        {
            WorkingGroup = workingGroup.Value;
        }
    }
}
