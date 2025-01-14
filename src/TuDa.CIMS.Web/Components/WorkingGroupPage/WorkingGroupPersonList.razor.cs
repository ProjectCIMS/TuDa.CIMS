using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;
public partial class WorkingGroupPersonList(IWorkingGroupApi workingGroupApi) : ComponentBase
{
    public IEnumerable<Person> Persons = new List<Person>();



    [Parameter] public Guid WorkingGroupId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var WorkingGroup = await workingGroupApi.GetAsync(WorkingGroupId);
        Persons = WorkingGroup.Value.Students;
    }

    private void RemoveBuyer()
    {
        /// Wait for api functionality
    }


    private void AddBuyer()
    {
        // TODO: Implement AddBuyer with a dialog here
    }

}

