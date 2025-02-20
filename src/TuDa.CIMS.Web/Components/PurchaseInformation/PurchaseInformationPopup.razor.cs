using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.PurchaseInformation;

public partial class PurchaseInformationPopup
{
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;

    [Parameter]
    public Guid WorkingGroupId { get; set; }

    [Parameter]
    public Guid PurchaseId { get; set; }

    private  Purchase Purchase { get; set; } = null!;

    //private readonly IPurchaseApi _purchaseApi = null!;

    // protected override async Task OnInitializedAsync()
    // {
    //     var purchase = await _purchaseApi.GetAsync(WorkingGroupId, PurchaseId);
    //     purchase.Switch(
    //         val => Purchase = val,
    //         err =>
    //             throw new InvalidOperationException(
    //                 $"Could not retrieve Purchase {PurchaseId} of WorkingGroup {WorkingGroupId}. Reason: {string.Join(",", err.Select(e => e.Description))}"
    //             )
    //     );
    // }

    private void GoToInvalidation()
    {
        _navigationManager.NavigateTo($"/shop/{PurchaseId}");
    }

}
