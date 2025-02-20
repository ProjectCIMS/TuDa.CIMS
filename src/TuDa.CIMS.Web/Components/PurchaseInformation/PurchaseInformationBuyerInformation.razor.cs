using System.Text;
using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.PurchaseInformation;

public partial class PurchaseInformationBuyerInformation
{
    [Parameter]
    public Guid WorkingGroupId { get; set; }

    [Parameter]
    public Guid PurchaseId { get; set; }

    //private readonly IPurchaseApi _purchaseApi;

    // public PurchaseInformationBuyerInformation(IPurchaseApi purchaseApi)
    // {
    //     _purchaseApi = purchaseApi;
    // }
    [CascadingParameter]
    private Purchase Purchase { get; set; } = new() { Buyer = new Professor { Name = "Mülller", FirstName = "Peter" } };

    private string SignatureAsBase64 => Encoding.UTF8.GetString(Purchase.Signature);


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
}
