using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Models;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class InvoicePage
{
    /// <summary>
    /// The id of the working group.
    /// </summary>
    [Parameter]
    public Guid WorkingGroupId { get; set; }

    private readonly IJSRuntime _jsRuntime;
    private readonly ISnackbar _snackbar;
    private readonly IWorkingGroupApi _workingGroupApi;
    private readonly IInvoiceApi _invoiceApi;

    public InvoicePage(
        IJSRuntime jsRuntime,
        IWorkingGroupApi workingGroupApi,
        IInvoiceApi invoiceApi,
        ISnackbar snackbar
    )
    {
        _jsRuntime = jsRuntime;
        _workingGroupApi = workingGroupApi;
        _invoiceApi = invoiceApi;
        _snackbar = snackbar;
    }

    private WorkingGroupResponseDto? _workingGroup { get; set; }

    /// <summary>
    /// Contains the title and the name of the buyer.
    /// </summary>
    private string BuyerString =>
        $"{_workingGroup?.Professor.Title} {_workingGroup?.Professor.Name}";

    /// <summary>
    /// Selected date range.
    /// </summary>
    private DateRange _dateRange { get; set; } = new();

    private string SelectedInvoiceNumber { get; set; } = string.Empty;

    private (DateOnly? Start, DateOnly? End) DateOnlyRange =>
        (
            _dateRange.Start is not null ? DateOnly.FromDateTime(_dateRange.Start.Value) : null,
            _dateRange.End is not null ? DateOnly.FromDateTime(_dateRange.End.Value) : null
        );

    /// <summary>
    /// The invoice statistics of the purchase.
    /// </summary>
    private InvoiceStatistics InvoiceStatistics { get; set; } =
        new()
        {
            TotalPriceConsumables = 0,
            TotalPriceChemicals = 0,
            TotalPriceSolvents = 0,
            TotalPriceGasCylinders = 0,
        };

    /// <summary>
    /// The TotalPriceSolvents text.
    /// </summary>
    private string TotalPriceSolvents => $"{InvoiceStatistics.TotalPriceSolvents:C}";

    /// <summary>
    /// The TotalPriceChemicals text.
    /// </summary>
    private string TotalPriceChemicals => $"{InvoiceStatistics.TotalPriceChemicals:C}";

    /// <summary>
    /// The TotalPriceConsumables text.
    /// </summary>
    private string TotalPriceConsumables => $"{InvoiceStatistics.TotalPriceConsumables:C}";

    /// <summary>
    /// The TotalPriceGasCylinders text.
    /// </summary>
    private string TotalPriceGasCylinders => $"{InvoiceStatistics.TotalPriceGasCylinders:C}";

    protected override async Task OnInitializedAsync()
    {
        var workingGroupErrorOr = await _workingGroupApi.GetAsync(WorkingGroupId);
        if (workingGroupErrorOr.IsError) { }

        _workingGroup = workingGroupErrorOr.Value;
    }

    /// <summary>
    /// Sets the invoice statistics.
    /// </summary>
    private async Task SetInvoiceStatistics()
    {
        var dates = DateOnlyRange;
        var errorOrStatistics = await _invoiceApi.GetStatisticsAsync(
            WorkingGroupId,
            dates.Start,
            dates.End
        );
        if (!errorOrStatistics.IsError)
        {
            InvoiceStatistics = errorOrStatistics.Value;
        }
    }

    private async Task OnDateRangeChanged(DateRange newDateRange)
    {
        _dateRange = newDateRange;
        await SetInvoiceStatistics();
    }

    private async Task OpenPdf()
    {
        var dates = DateOnlyRange;
        var wgId = WorkingGroupId;
        var infos = new AdditionalInvoiceInformation { InvoiceNumber = SelectedInvoiceNumber };
        var success = await _invoiceApi.OpenPdfAsync(
            wgId,
            infos,
            _jsRuntime,
            dates.Start,
            dates.End
        );

        if (success.IsError)
        {
            _snackbar.Add("Etwas hat bei der Erstellung nicht funktioniert.", Severity.Error);
        }
    }
}
