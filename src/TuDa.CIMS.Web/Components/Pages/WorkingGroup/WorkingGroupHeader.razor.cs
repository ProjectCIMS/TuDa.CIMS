using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.Pages.WorkingGroup;

public partial class WorkingGroupHeader : ComponentBase
{
    [Parameter] public required Professor Professor { get; set;}
 }
