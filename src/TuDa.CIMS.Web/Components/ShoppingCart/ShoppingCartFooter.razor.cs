﻿using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.ShoppingCart;

/// <summary>
/// Class for the shopping cart footer
/// </summary>

public partial class ShoppingCartFooter : ComponentBase
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
