using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using TuDa.CIMS.Shared.Models;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class InvoicePage
{
    [Inject] private IJSRuntime _jsRuntime { get; set; } = null!;
    [Inject] private IWorkingGroupApi _workingGroupApi { get; set; } = null!;
    [Inject] private IInvoiceApi _invoiceApi { get; set; } = null!;


    // Error when WorkingGroup is not static occurs.
    /// <summary>
    /// Contains the title and the name of the buyer.
    /// </summary>
    private string BuyerString =>
        $"{_workingGroupApi.GetAsync(workingGroupId).Result.Value.Professor.Title} " + " "
        + $"{_workingGroupApi.GetAsync(workingGroupId).Result.Value.Professor.Name}";

    /// <summary>
    /// Selected date range.
    /// </summary>
    private DateRange _dateRange { get; set; } = new();

    /// <summary>
    /// The id of the working group.
    /// </summary>
    private Guid workingGroupId { get; set; }

    /// <summary>
    /// The invoice statistics of the purchase.
    /// </summary>
    private InvoiceStatistics? InvoiceStatistics { get; set; }

    /// <summary>
    /// Sets the invoice statistics.
    /// </summary>
    private async Task SetInvoiceStatistics()
    {
        if (_dateRange is { Start: not null, End: not null })
        {
            var errorOrStatistics = await _invoiceApi.GetStatisticsAsync(workingGroupId,
                DateOnly.FromDateTime(_dateRange.Start.Value), DateOnly.FromDateTime(_dateRange.End.Value));
            if (!errorOrStatistics.IsError)
            {
                InvoiceStatistics = errorOrStatistics.Value;
            }
        }
    }

    private async Task OnDateRangeChanged(DateRange newDateRange)
    {
        _dateRange = newDateRange;
        await SetInvoiceStatistics();
    }

    private DateTime? SelectedDueDate { get; set; } = null!;

    private string SelectedInvoiceNumber { get; set; } = null!;

    private async Task OpenPdf(bool firstRender)
    {
        if (SelectedDueDate != null && SelectedInvoiceNumber.Length > 0)
        {
            var wgId = workingGroupId;
            var infos = new AdditionalInvoiceInformation
            {
                InvoiceNumber = SelectedInvoiceNumber, DueDate = DateOnly.FromDateTime(SelectedDueDate.Value),
            };
            var success = await _invoiceApi.OpenPdfAsync(
                wgId, infos
                ,
                _jsRuntime
            );
        }
    }
}
