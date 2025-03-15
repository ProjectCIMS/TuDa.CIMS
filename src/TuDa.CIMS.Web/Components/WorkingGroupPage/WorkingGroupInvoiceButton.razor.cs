using Microsoft.AspNetCore.Components;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public partial class WorkingGroupInvoiceButton : ComponentBase
{
    private readonly NavigationManager NavigationManager;

    public WorkingGroupInvoiceButton(NavigationManager navigationManager)
    {
        NavigationManager = navigationManager;
    }

    private void NavigateToInvoice()
    {
        NavigationManager.NavigateTo($"{NavigationManager.Uri}/invoice");
    }
}
