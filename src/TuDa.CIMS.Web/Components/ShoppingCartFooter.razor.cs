using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components;

/// <summary>
/// Class for the shopping cart footer
/// </summary>
/// <param name="dialogService"></param>
public partial class ShoppingCartFooter(IDialogService dialogService) : ComponentBase
{
    /// <summary>
    /// Parameter for the purchase
    /// </summary>
    [Parameter]
    public required Purchase Purchase { get; set; }

    /// <summary>
    /// Parameter for the checkout event
    /// </summary>
    [Parameter]
    public EventCallback OnCheckout { get; set; }

    /// <summary>
    /// The total price of the purchase
    /// </summary>
    public double TotalPrice => Purchase?.TotalPrice ?? 0.0;

    /// <summary>
    /// Opens the popup.
    /// </summary>
    private Task OpenDialogAsync()
    {
        return dialogService.ShowAsync<ShoppingCartSubmitPopup>("Custom Options Dialog",
            new DialogOptions() { CloseButton = true });
    }

    /// <summary>
    /// Initialize OnCheckout EventCallback so that OpenDialogAsync is called.
    /// </summary>
    protected override void OnInitialized()
    {
        OnCheckout = EventCallback.Factory.Create(this, OpenDialogAsync);
    }
}
