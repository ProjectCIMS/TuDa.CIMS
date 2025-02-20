using System.Globalization;
using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Models;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.PurchaseInformation;

public partial class PurchaseInformationStatistics
{
    [Parameter] public Guid WorkingGroupId { get; set; }

    [Parameter] public Guid PurchaseId { get; set; }

    private readonly IPurchaseApi _purchaseApi = null!;


    [CascadingParameter] private Purchase Purchase { get; set; } = null!;

    public PurchaseInformationStatistics(IPurchaseApi purchaseApi)
    {
        _purchaseApi = purchaseApi;
    }

    /*protected override async Task OnInitializedAsync()
    {
        var purchase = await _purchaseApi.GetAsync(WorkingGroupId, PurchaseId);
        purchase.Switch(
            val => Purchase = val,
            err =>
                throw new InvalidOperationException(
                    $"Could not retrieve Purchase {PurchaseId} of WorkingGroup {WorkingGroupId}. " +
                    $"Reason:{string.Join(",", err.Select(e => e.Description))}"
                )
        );
    }*/

    /// <summary>
    /// Returns the TotalPriceChemicals text.
    /// </summary>
    private string GetTotalPriceChemicals()
    {
        double sum = Purchase.Entries.Where(entry => entry.AssetItem is Chemical and not Solvent)
            .Sum(entry => entry.AssetItem.Price);
        return sum.ToString("C", CultureInfo.GetCultureInfo("de-DE"));
    }

    /// <summary>
    /// Returns the TotalPriceSolvents text.
    /// </summary>
    private string GetTotalPriceSolvents()
    {
        double sum = Purchase.Entries.Where(entry => entry.AssetItem is Solvent)
            .Sum(entry => entry.AssetItem.Price);
        return sum.ToString("C", CultureInfo.GetCultureInfo("de-DE"));
    }

    /// <summary>
    /// Returns the TotalPriceGasCylinders text.
    /// </summary>
    private string GetTotalPriceGasCylinders()
    {
        double sum = Purchase.Entries.Where(entry => entry.AssetItem is GasCylinder)
            .Sum(entry => entry.AssetItem.Price);
        return sum.ToString("C", CultureInfo.GetCultureInfo("de-DE"));
    }

    /// <summary>
    /// Returns the TotalPriceConsumables text.
    /// </summary>
    private string GetTotalPriceConsumables()
    {
        double sum = Purchase.Entries.Where(entry => entry.AssetItem is Consumable)
            .Sum(entry => entry.AssetItem.Price);
        return sum.ToString("C", CultureInfo.GetCultureInfo("de-DE"));
    }

    /// <summary>
    /// Returns the TotalPrice text.
    /// </summary>
    private string GetTotalPrice()
    {
        return Purchase.TotalPrice.ToString("C", CultureInfo.GetCultureInfo("de-DE"));
    }
}
