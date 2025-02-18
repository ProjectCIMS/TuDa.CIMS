using System.Globalization;
using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Models;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.PurchaseInformation;

public partial class PurchaseInformationStatistics
{
    [Parameter]
    public Guid WorkingGroupId { get; set; }

    [Parameter]
    public Guid PurchaseId { get; set; }

    private readonly IPurchaseApi _purchaseApi = null!;

    private readonly IInvoiceApi _invoiceApi = null!;

    private Purchase Purchase { get; set; } = null!;

    public PurchaseInformationStatistics(IPurchaseApi purchaseApi, IInvoiceApi invoiceApi)
    {
        _purchaseApi = purchaseApi;
        _invoiceApi = invoiceApi;
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
    /// The invoice statistics of the purchase.
    /// </summary>
    private InvoiceStatistics? InvoiceStatistics { get; set; } =
        new()
        {
            TotalPriceConsumables = 0,
            TotalPriceChemicals = 0,
            TotalPriceSolvents = 0,
            TotalPriceGasCylinders = 0,
        };

    /// <summary>
    /// Sets the invoice statistics.
    /// </summary>
    private async Task SetInvoiceStatistics()
    {
        if (Purchase.CompletionDate is not null)
        {
            var errorOrStatistics = await _invoiceApi.GetStatisticsAsync(
                WorkingGroupId,
                DateOnly.FromDateTime(Purchase.CreatedAt),
                // CompletionDate probably wrong here
                DateOnly.FromDateTime((DateTime)Purchase.CompletionDate)
            );
            if (!errorOrStatistics.IsError)
            {
                InvoiceStatistics = errorOrStatistics.Value;
            }
        }
    }

    /// <summary>
    /// Returns the TotalPriceChemicals text.
    /// </summary>
    private string GetTotalPriceChemicals()
    {
        return InvoiceStatistics?.TotalPriceChemicals.ToString(
            "0.00",
            CultureInfo.GetCultureInfo("de-DE")
        ) + "€";
    }

    /// <summary>
    /// Returns the TotalPriceSolvents text.
    /// </summary>
    private string GetTotalPriceSolvents()
    {
        return InvoiceStatistics?.TotalPriceSolvents.ToString(
            "0.00",
            CultureInfo.GetCultureInfo("de-DE")
        ) + "€";
    }

    /// <summary>
    /// Returns the TotalPriceGasCylinders text.
    /// </summary>
    private string GetTotalPriceGasCylinders()
    {
        return InvoiceStatistics?.TotalPriceGasCylinders.ToString(
            "0.00",
            CultureInfo.GetCultureInfo("de-DE")
        ) + "€";
    }

    /// <summary>
    /// Returns the TotalPrice text.
    /// </summary>
    private string GetTotalPrice()
    {
        return Purchase.TotalPrice.ToString(
            "0.00",
            CultureInfo.GetCultureInfo("de-DE")
        ) + "€";
    }

    /// <summary>
    /// Returns the TotalPriceConsumables text.
    /// </summary>
    private string GetTotalPriceConsumables()
    {
        return InvoiceStatistics?.TotalPriceConsumables.ToString(
            "0.00",
            CultureInfo.GetCultureInfo("de-DE")
        ) + "€";
    }

}
