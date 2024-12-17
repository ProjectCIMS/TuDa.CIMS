using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Web.Components;

public partial class ShoppingCartSubmitPopup
{
    /// <summary>
    /// Returns the name of a given working group.
    /// </summary>
    private static string ToString(WorkingGroup workingGroup) =>
        workingGroup switch
        {
            null => "",
            _ => workingGroup.Professor.Name,
        };

    public WorkingGroup WorkingGroup { get; set; }

    [Parameter]
    public List<WorkingGroup> WorkingGroups { get; set; }

    /// <summary>
    /// Search for the selection of the working group.
    /// </summary>
    private Task<IEnumerable<WorkingGroup>> Search(string searchText, CancellationToken cancellationToken)
    {
        return Task.FromResult(WorkingGroups
            .Where(w =>
                string.IsNullOrWhiteSpace(searchText) ||
                w.Professor.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)));
    }

    /// <summary>
    /// CascadingParamter MudDialag.
    /// </summary>
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; }

    /// <summary>
    /// Closes the MudDialog.
    /// </summary>
    private void Submit() => MudDialog.Close(DialogResult.Ok(WorkingGroup));

    /// <summary>
    /// Cancels the MudDialog.
    /// </summary>
    private void Cancel() => MudDialog.Cancel();

    /// <summary>
    /// List of purchase entries to be shown.
    /// </summary>
    [Parameter]
    public static List<PurchaseEntry> PurchaseEntries { get; set; }

    /// <summary>
    /// Returns the amount with the respective price unit of the given purchase entry as a string.
    /// </summary>
    private static string GetAmountText(PurchaseEntry purchaseEntry) =>
        $"{purchaseEntry.Amount}" + purchaseEntry.AssetItem switch
        {
            Substance substance => substance.PriceUnit switch
            {
                PriceUnits.PerKilo => " kg",
                PriceUnits.PerLiter => " l",
                PriceUnits.PerPiece => " Stück",
                _ => $" {substance.PriceUnit}"
            },
            _ => " Stück"
        };
}
