using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Models;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class InvoicePage
{
    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = null!;

    [Inject]
    private IWorkingGroupApi _workingGroupApi { get; set; } = null!;

    [Inject]
    private IInvoiceApi _invoiceApi { get; set; } = null!;

    private WorkingGroup? _workingGroup { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var workingGroupErrorOr = await _workingGroupApi.GetAsync(workingGroupId);
        if (workingGroupErrorOr.IsError) { }

        _workingGroup = workingGroupErrorOr.Value;
    }

    /// <summary>
    /// Contains the title and the name of the buyer.
    /// </summary>
    private string BuyerString =>
        $"{_workingGroup?.Professor.Title} {_workingGroup?.Professor.Name}";

    /// <summary>
    /// Selected date range.
    /// </summary>
    private DateRange _dateRange { get; set; } = new();

    /// <summary>
    /// The id of the working group.
    /// </summary>
    [Parameter]
    public Guid workingGroupId { get; set; }

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
        if (_dateRange is { Start: not null, End: not null })
        {
            var errorOrStatistics = await _invoiceApi.GetStatisticsAsync(
                workingGroupId,
                DateOnly.FromDateTime(_dateRange.Start.Value),
                DateOnly.FromDateTime(_dateRange.End.Value)
            );
            if (!errorOrStatistics.IsError)
            {
                InvoiceStatistics = errorOrStatistics.Value;
            }
        }
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
    /// Returns the TotalPriceConsumables text.
    /// </summary>
    private string GetTotalPriceConsumables()
    {
        return InvoiceStatistics?.TotalPriceConsumables.ToString(
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

    private async Task OnDateRangeChanged(DateRange newDateRange)
    {
        _dateRange = newDateRange;
        await SetInvoiceStatistics();
    }

    private string SelectedInvoiceNumber { get; set; } = null!;

    private async Task OpenPdf()
    {
        if (SelectedInvoiceNumber.Length > 0)
        {
            var wgId = workingGroupId;
            var infos = new AdditionalInvoiceInformation { InvoiceNumber = SelectedInvoiceNumber };
            var success = await _invoiceApi.OpenPdfAsync(wgId, infos, _jsRuntime);
        }
    }
}
