using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.ShoppingCart;

public partial class ShoppingCartSubmitPopup
{
    /// <summary>
    /// PurchaseEntries of the Purchase.
    /// </summary>
    [Parameter]
    public required List<PurchaseEntry> PurchaseEntries { get; set; }

    /// <summary>
    /// List of available WorkingGroups.
    /// TODO: Replace with WorkingGroupApi and move to WorkingGroupSelection.
    /// </summary>
    [Parameter]
    public required List<WorkingGroup> WorkingGroups { get; set; }

    /// <summary>
    /// CascadingParamter MudDialag.
    /// </summary>
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    /// <summary>
    /// The selected working group.
    /// </summary>
    private WorkingGroup WorkingGroup { get; set; } = new ()
    {
        Professor = new Professor
        {
            Name = "Mustermann",
            FirstName = "Max"
        },
        Students = new()
        {
            new Student { Name = "Alice" },
            new Student { Name = "Bob" }
        }
    };

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
    private void Submit() => MudDialog.Close(DialogResult.Ok(WorkingGroup));
}
