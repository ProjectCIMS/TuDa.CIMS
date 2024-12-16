using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components;

/// <summary>
/// Class for the shopping cart footer
/// </summary>
/// <param name="dialogService"></param>
public partial class ShoppingCartFooter() : ComponentBase
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
}
