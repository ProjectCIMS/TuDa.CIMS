using Microsoft.AspNetCore.Components;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;
public partial class WorkingGroupInvoiceButton : ComponentBase
{
    [Parameter] public EventCallback Checkout { get; set; }

    // TODO Navigate to the bills page
}

