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
        var workingGroup = await workingGroupApi.GetAsync(WorkingGroupId);
        Persons = workingGroup.Value.Students;
    }

    private void RemoveBuyer()
    {
        /// Wait for api functionality

        if (Persons.Any())
        {
            var modifiableList = Persons.ToList();
            modifiableList.RemoveAt(modifiableList.Count - 1);
            Persons = modifiableList;
        }
    }


    private void AddBuyer()
    {
        // TODO: Implement AddBuyer with a dialog here
    }

}

