using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Dtos.Responses;

namespace TuDa.CIMS.Web.Components.PurchaseInformation;

public partial class PurchaseInformationPopup
{
    private readonly NavigationManager _navigationManager;

    public PurchaseInformationPopup(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    [Parameter]
    public Guid WorkingGroupId { get; set; }

    [Parameter]
    public PurchaseResponseDto Purchase { get; set; } = null!;

    [Parameter]
    public string SignatureAsBase64 { get; set; } = string.Empty;

    private void GoToInvalidation()
    {
        _navigationManager.NavigateTo($"/shop/{WorkingGroupId}/{Purchase.Id}");
    }

    private string CompletionDateString =>
        Purchase.CompletionDate?.ToLocalTime().ToString("dd.MM.yyyy") ?? "";
}
