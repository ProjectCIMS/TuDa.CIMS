using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.Pages.WorkingGroup;

public partial class WorkingGroupPersonList : ComponentBase
{
    public List<Person> Persons{get;set;} = [];

    private List<string> items = new List<string> { "Matse Müller", "Herbert Grönemeyer" };

    private void AddItem()
    {
        items.Add($"Neues Item {items.Count + 1}");
    }
}

