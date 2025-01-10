using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;
public partial class WorkingGroupHeader : ComponentBase
{
    [Parameter] public required Professor Professor { get; set;}

    public required Guid workingGroupId { get; set; }


    public void EditProfessor()
    {
        /// use API update function here
    }
 }
