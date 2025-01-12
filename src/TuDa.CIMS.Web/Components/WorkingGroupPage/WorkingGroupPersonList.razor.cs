using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;
public partial class WorkingGroupPersonList(IWorkingGroupApi _workingGroupApi) : ComponentBase
{
    public List<Person> Persons = new List<Person>();

    public WorkingGroup WorkingGroup { get; set; }

    private Guid WorkingGroupId { get; set; }

    private void RemoveBuyer()
    {
        /// Wait for api functionality
        /// _workingGroupApi.Remove(WorkingGroupId, person.Id);
    }


    private void AddBuyer()
    {
        // TODO: Implement AddBuyer with a dialog here
    }

    private List<string> items = new List<string> { "Matse Müller", "Herbert Grönemeyer" };

    private void AddItem()
    {
        items.Add($"Neues Item {items.Count + 1}");
    }
}

