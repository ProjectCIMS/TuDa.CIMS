using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Shared.Models;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class InvoicePage
{
    // Error when Purchase is not static occurs.
    /// <summary>
    /// Contains the title and the name of the buyer.
    /// </summary>
    private readonly string BuyerString = (Purchase.Buyer is Professor professor ? professor.Title : string.Empty).ToUpper()
                                     + " " + Purchase.Buyer.Name.ToUpper();

    /// <summary>
    /// Selected date range.
    /// </summary>
    private DateRange _dateRange { get; set; } = new();

    /// <summary>
    /// Purchase with purchase entries.
    /// </summary>

    public static Purchase Purchase { get; set; }

    /// <summary>
    /// The id of the working group.
    /// </summary>
    private Guid WorkingGroupId { get; set; } = Guid.Empty;

    /// <summary>
    /// The invoice statistics of the purchase.
    /// </summary>
    private InvoiceStatistics InvoiceStatistics { get; set; }
    // = await _invoiceApi.GetStatisticsAsync(WorkingGroupId,
    //    _dateRange.Start,
    //     _dateRange.End);


    [Inject] private IJSRuntime _jsRuntime { get; set; } = null!;
    [Inject] private IWorkingGroupApi _workingGroupApi { get; set; } = null!;
    // [Inject] private IInvoiceApi _invoiceApi { get; set; } = null!;

    private DateTime? SelectedDueDate { get; set; } = null!;

    private string SelectedInvoiceNumber { get; set; } = null!;

    /*protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var workingGroups = await _workingGroupApi.GetAllAsync();

        if (!workingGroups.IsError)
        {
            var wgId = WorkingGroupId;
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
