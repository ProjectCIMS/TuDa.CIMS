using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;
public partial class WorkingGroupPersonList : ComponentBase
{
    public List<Person> Persons = new List<Person>();


    static Guid selectedPersonId = Guid.Empty;

    private void EditBuyer()
    {
        // do something
    }

    private void RemoveBuyer()
    {
        // do something
    }

    private void AddBuyer()
    {
        // do something
    }

    private List<string> items = new List<string> { "Matse Müller", "Herbert Grönemeyer" };

    private void AddItem()
    {
        items.Add($"Neues Item {items.Count + 1}");
    }
}

