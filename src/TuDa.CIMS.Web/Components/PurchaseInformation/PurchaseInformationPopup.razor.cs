using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.PurchaseInformation;

public partial class PurchaseInformationPopup
{
    private NavigationManager _navigationManager { get; set; }
    private IPurchaseApi _purchaseApi { get; set; }

    [Parameter]
    public Guid WorkingGroupId { get; set; }

    [Parameter]
    public Guid PurchaseId { get; set; }
    public PurchaseInformationPopup(NavigationManager navigationManager, IPurchaseApi purchaseApi)
    {
        _navigationManager = navigationManager;
        _purchaseApi = purchaseApi;
    }
    private  PurchaseResponseDto Purchase { get; set; } = null!;
    protected override async Task OnInitializedAsync()
     {
         var purchase = await _purchaseApi.GetAsync(WorkingGroupId, PurchaseId);
         purchase.Switch(
             val => Purchase = val,
             err =>
                 throw new InvalidOperationException(
                     $"Could not retrieve Purchase {PurchaseId} of WorkingGroup {WorkingGroupId}. Reason: " +
                     $"{string.Join(",", err.Select(e => e.Description))}"
                 )
         );
     }
    private void GoToInvalidation()
    {
        _navigationManager.NavigateTo($"/shop/{PurchaseId}");
    }

}
