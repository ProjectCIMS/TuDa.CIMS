using Microsoft.AspNetCore.Components;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class WorkingGroupPage : ComponentBase
{
    [Parameter] public Guid WorkingGroupId { get; set; }
}

