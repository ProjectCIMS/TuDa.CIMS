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

    private bool isWorkingGroupValid;
    private bool isBuyerValid;
    private bool isFormValid => isWorkingGroupValid && isBuyerValid;

    private void HandleWorkingGroupValidationChanged(bool isValid)
    {
        isWorkingGroupValid = isValid;
        StateHasChanged();
    }

    private void HandleBuyerValidationChanged(bool isValid)
    {
        isBuyerValid = isValid;
        StateHasChanged();
    }
    private string buyerValidationMessage { get; set; }

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
        if (WorkingGroup?.Professor.Id == Buyer.Id || (bool)WorkingGroup?.Students.Contains(Buyer))
        {
            MudDialog.Close(DialogResult.Ok(new WorkingGroupWithBuyer(WorkingGroup!.Id, Buyer.Id)));
        }
        else
        {
            isBuyerValid = false;
            buyerValidationMessage = "Der ausgewählte Käufer gehört nicht zur ausgewählten Arbeitsgruppe.";
        }
    }
}
