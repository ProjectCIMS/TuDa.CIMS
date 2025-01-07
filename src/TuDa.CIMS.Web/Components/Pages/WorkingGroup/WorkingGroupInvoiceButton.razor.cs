using Microsoft.AspNetCore.Components;

namespace TuDa.CIMS.Web.Components.Pages.WorkingGroup;

public partial class WorkingGroupInvoiceButton : ComponentBase
{
    [Parameter] public EventCallback Checkout { get; set; }
}

