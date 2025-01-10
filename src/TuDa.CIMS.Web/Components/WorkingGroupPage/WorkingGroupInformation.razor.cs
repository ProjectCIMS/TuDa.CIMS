using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public partial class WorkingGroupInformation : ComponentBase
{
    [Parameter] public required  Professor ProfessorInfo { get; set; }
}

