using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public partial class WorkingGroupPurchaseList(IPurchaseApi _iPurchaseApi) : ComponentBase
{
    [Parameter] public IEnumerable<Purchase> Purchases { get; set; } = [];

    private Guid WorkingGroupId { get; set; }

    /*
    protected override async Task OnInitializedAsync()
    {
        var purchases = await _iPurchaseApi.GetAllAsync(WorkingGroupId);
        Purchases = purchases.Value;
    }
    */


    public void NavigateToPurchase()
    {
        // TODO: Implement Navigation to Purchase
    }

}

