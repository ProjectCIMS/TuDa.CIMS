using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;
public partial class WorkingGroupPersonList(IWorkingGroupApi workingGroupApi) : ComponentBase
{

    private IEnumerable<Person> _persons = new List<Person>();

    [Parameter] public Guid WorkingGroupId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var workingGroup = await workingGroupApi.GetAsync(WorkingGroupId);
        _persons = workingGroup.Value.Students;
    }

    private void RemoveBuyer()
    {
        /// Wait for api functionality

        if (_persons.Any())
        {
            var modifiableList = _persons.ToList();
            modifiableList.RemoveAt(modifiableList.Count - 1);
            _persons = modifiableList;
        }
    }


    private void AddBuyer()
    {
        // TODO: Implement AddBuyer with a dialog here
    }

}

