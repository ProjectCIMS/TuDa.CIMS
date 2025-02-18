using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.PurchaseInformation;

public partial class PurchaseInformationPopup
{
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Parameter]
    public Guid WorkingGroupId { get; set; }

    [Parameter]
    public Guid PurchaseId { get; set; }

    private  Purchase Purchase { get; set; } = null!;



    private void GoToInvalidation()
    {
        _navigationManager.NavigateTo($"/shop/{PurchaseId}");
    }

}
