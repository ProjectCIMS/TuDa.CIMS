using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Web.Components;

public partial class ShoppingCartSubmitPopupContent
{
    /// <summary>
    /// CascadingParameter for working group.
    /// </summary>
    [CascadingParameter]
    public WorkingGroup WorkingGroup { get; set; }

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
    /// Returns the name of a given working group.
    /// </summary>
    private static string ToString(WorkingGroup workingGroup) =>
        workingGroup switch
        {
            null => "",
            _ => workingGroup.Professor.Name,
        };

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

    /// <summary>
    /// List of working groups.
    /// </summary>
    [Parameter]
    public List<WorkingGroup> WorkingGroups { get; set; }
}
