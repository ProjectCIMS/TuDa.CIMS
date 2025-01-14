using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Models;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class InvoicePage
{
    /// <summary>
    /// Parameter for the working group.
    /// </summary>
    [Parameter]
    public static WorkingGroup? WorkingGroup { get; set; }

    // Error when WorkingGroup is not static occurs.
    /// <summary>
    /// Contains the title and the name of the buyer.
    /// </summary>
    private readonly string BuyerString = WorkingGroup.Professor.Title + " " + WorkingGroup.Professor.Name;

    /// <summary>
    /// Selected date range.
    /// </summary>
    private DateRange _dateRange { get; set; } = new();

    /// <summary>
    /// The id of the working group.
    /// </summary>
    private Guid workingGroupId { get; set; } = WorkingGroup.Id;

    /// <summary>
    /// The invoice statistics of the purchase.
    /// </summary>
    private InvoiceStatistics InvoiceStatistics { get; set; } = null!;

    // /// <summary>
    // /// Sets the invoice statistics.
    // /// </summary>
    // private void SetInvoiceStatistics()
    // {
    //     InvoiceStatistics = await _invoiceApi.GetStatisticsAsync(workingGroupId,
    //         _dateRange.Start, _dateRange.End);
    // }


    [Inject] private IJSRuntime _jsRuntime { get; set; } = null!;
    [Inject] private IWorkingGroupApi _workingGroupApi { get; set; } = null!;
    // [Inject] private IInvoiceApi _invoiceApi { get; set; } = null!;

    private DateTime? SelectedDueDate { get; set; } = null!;

    private string SelectedInvoiceNumber { get; set; } = null!;

    /*private async Task OpenPdf(bool firstRender)
    {
        if (!workingGroupId.IsError)
        {
            var wgId = workingGroupId;
            var infos = new AdditionalInvoiceInformation
            {
                InvoiceNumber = SelectedInvoiceNumber,
                DueDate = SelectedDueDate,
            };

            var success = await _invoiceApi.OpenPdfAsync(
                wgId,infos
                ,
                _jsRuntime
            );
        }
    }*/
}
