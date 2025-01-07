using Microsoft.AspNetCore.Components;

namespace TuDa.CIMS.Web.Components.Pages.WorkingGroup;

public partial class WorkingGroupPersonList : ComponentBase
{

    private List<string> items = new List<string> { "Matse Müller", "Herbert Grönemeyer" };

    private void AddItem()
    {
        items.Add($"Neues Item {items.Count + 1}");
    }
}

