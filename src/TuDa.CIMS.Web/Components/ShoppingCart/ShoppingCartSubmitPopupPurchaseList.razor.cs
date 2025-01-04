using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Web.Components.ShoppingCart;

public partial class ShoppingCartSubmitPopupPurchaseList
{
    /// <summary>
    /// List of purchase entries to be shown.
    /// </summary>
    [Parameter]
    public required List<PurchaseEntry> PurchaseEntries { get; set; }

    /// <summary>
    /// Returns the amount with the respective price unit of the given purchase entry as a string.
    /// </summary>
    private static string GetAmountText(PurchaseEntry purchaseEntry) =>
        $"{purchaseEntry.Amount}"
        + purchaseEntry.AssetItem switch
        {
            Substance substance => substance.PriceUnit switch
            {
                MeasurementUnits.KiloGram => " kg",
                MeasurementUnits.Liter => " l",
                MeasurementUnits.Piece => " Stück",
                MeasurementUnits.Gram => " g",
                MeasurementUnits.MilliLiter => " ml",
                _ => $" {substance.PriceUnit}",
            },
            _ => " Stück",
        };
}
