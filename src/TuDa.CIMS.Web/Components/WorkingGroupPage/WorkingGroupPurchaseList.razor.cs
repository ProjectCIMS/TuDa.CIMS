using System.Globalization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Web.Components.PurchaseInformation;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public partial class WorkingGroupPurchaseList
{
    private readonly IDialogService _dialogService;
    private readonly NavigationManager _navigation;
    private readonly IPurchaseApi _iPurchaseApi;

    public WorkingGroupPurchaseList(
        IDialogService dialogService,
        NavigationManager navigationManager,
        IPurchaseApi iPurchaseApi
    )
    {
        _dialogService = dialogService;
        _navigation = navigationManager;
        _iPurchaseApi = iPurchaseApi;
    }
    [Parameter] public IEnumerable<PurchaseResponseDto> Purchases { get; set; } = [];

    [Parameter] public Guid WorkingGroupId { get; set; }

    private IEnumerable<PurchaseResponseDto> SortedPurchases =>
        Purchases.OrderByDescending(p => p.CompletionDate);


    protected override async Task OnInitializedAsync()
    {
        var purchases = await _iPurchaseApi.GetAllAsync(WorkingGroupId);
        Purchases = purchases.Value;
    }

    /// <summary>
    /// Formats the completion date of a purchase in the wished form.
    /// </summary>
    /// <param name="value">The chosen purchase</param>
    /// <returns>Returns the formatted date as string</returns>
    private static string FormatCompletionDate(PurchaseResponseDto? value)
    {
        return value!.CompletionDate.HasValue
            ? value.CompletionDate.Value.ToString(
                "dd.MM.yyyy HH:mm:ss",
                CultureInfo.GetCultureInfo("de-DE")
            )
            : "";
    }

    private async Task NavigateToPurchase(PurchaseResponseDto purchase)
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };

        // Set Parameters
        var parameters = new DialogParameters
        {
            { "WorkingGroupId", WorkingGroupId }, { "Purchase", purchase },
            {"Signature", _iPurchaseApi.RetrieveSignatureAsync(WorkingGroupId, purchase.Id)}
        };
        await _dialogService.ShowAsync<PurchaseInformationPopup>(
            "Rechnungsinformationen",
            parameters,
            options
        );
    }
}
