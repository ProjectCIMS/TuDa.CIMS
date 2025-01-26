using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Helper;

namespace TuDa.CIMS.Web.Components.ShoppingCart;

public partial class ShoppingCartSubmitPopup
{
    /// <summary>
    /// PurchaseEntries of the Purchase.
    /// </summary>
    [Parameter]
    public required List<PurchaseEntry> PurchaseEntries { get; set; }

    /// <summary>
    /// CascadingParameter MudDialog.
    /// </summary>
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    private bool IsValid => WorkingGroupIsValid && BuyerIsValid;

    public bool WorkingGroupIsValid { get; set;}

    public bool BuyerIsValid { get; set; }

    /// <summary>
    /// The selected working group.
    /// </summary>
    public WorkingGroup? WorkingGroup { get; set; }

    /// <summary>
    /// The selected buyer.
    /// </summary>
    private Person Buyer { get; set; } = null!;

    /// <summary>
    /// Cancels the MudDialog.
    /// </summary>
    private void Cancel() => MudDialog.Cancel();

    /// <summary>
    /// Closes the MudDialog.
    /// </summary>
    private void Submit()
    {
        if (IsValid)
        {
            MudDialog.Close(DialogResult.Ok(new WorkingGroupWithBuyer(WorkingGroup!.Id, Buyer.Id)));
        }
    }
}
