using Microsoft.AspNetCore.Components;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;
public partial class WorkingGroupInvoiceButton : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    private void NavigateToInvoice()
    {
        NavigationManager.NavigateTo($"{NavigationManager.Uri}/invoice");
    }

}

