using System.Globalization;
using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public partial class WorkingGroupPurchaseList(IPurchaseApi _iPurchaseApi) : ComponentBase
{
    [Parameter]
    public IEnumerable<PurchaseResponseDto> Purchases { get; set; } = [];

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
    private string FormatCompletionDate(PurchaseResponseDto? value)
    {
        return value!.CompletionDate.HasValue
            ? value.CompletionDate.Value.ToString(
                "dd.MM.yyyy HH:mm:ss",
                CultureInfo.GetCultureInfo("de-DE")
            )
            : "";
    }

    public void NavigateToPurchase()
    {
        // TODO: Implement Navigation to Purchase
    }
}
