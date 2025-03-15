using System.Globalization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Web.Components.PurchaseInformation;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public partial class WorkingGroupPurchaseList
{
    /// <summary>
    /// List of purchases that will be displayed.
    /// </summary>
    [Parameter]
    public List<PurchaseResponseDto> Purchases { get; set; } = [];

    /// <summary>
    /// Working group ID to which the purchases belong to.
    /// </summary>
    [Parameter]
    public Guid WorkingGroupId { get; set; }

    private readonly IDialogService _dialogService;
    private readonly NavigationManager _navigation;
    private readonly IPurchaseApi _purchaseApi;

    public WorkingGroupPurchaseList(
        IDialogService dialogService,
        NavigationManager navigationManager,
        IPurchaseApi iPurchaseApi
    )
    {
        _dialogService = dialogService;
        _navigation = navigationManager;
        _purchaseApi = iPurchaseApi;
    }

    private IEnumerable<PurchaseResponseDto> SortedPurchases =>
        Purchases.OrderByDescending(p => p.CompletionDate);

    /// <summary>
    /// Formats the completion date of a purchase in the wished form.
    /// </summary>
    /// <param name="value">The chosen purchase</param>
    /// <returns>Returns the formatted date as string</returns>
    private static string FormatCompletionDate(PurchaseResponseDto? value)
    {
        return value
                ?.CompletionDate?.ToLocalTime()
                .ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.GetCultureInfo("de-DE")) ?? "";
    }

    private static string GetTextStyle(PurchaseResponseDto purchase) =>
        purchase.Invalidated ? $"color: {Colors.Red.Default}; text-decoration: line-through;" : "";

    private async Task OpenPurchaseInfo(PurchaseResponseDto purchase)
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        var signature = await _purchaseApi
            .RetrieveSignatureAsync(WorkingGroupId, purchase.Id)
            .Else("");

        var parameters = new DialogParameters<PurchaseInformationPopup>
        {
            { popup => popup.WorkingGroupId, WorkingGroupId },
            { popup => popup.Purchase, purchase },
            { popup => popup.SignatureAsBase64, signature.Value },
        };

        await _dialogService.ShowAsync<PurchaseInformationPopup>(
            "Rechnungsinformationen",
            parameters,
            options
        );
    }
}
