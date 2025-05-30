using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Web.Components.ShoppingCart;

public partial class ShoppingCartProductDialog
{
    private readonly ILogger<ShoppingCartProductDialog> _logger;

    public ShoppingCartProductDialog(ILogger<ShoppingCartProductDialog> logger) => _logger = logger;

    [CascadingParameter]
    public required MudDialogInstance ProductDialog { get; set; }

    // Product to be shown in Dialog.
    [Parameter]
    public required AssetItem Product { get; set; }

    // Amount of Product
    public double Amount { get; set; } = 1.0;
    public int AmountInt { get; set; } = 1;

    /// <summary>
    /// checks if the amount should be inputted as int or double
    /// </summary>
    /// <returns>returns true if it should be an integer</returns>
    public bool IsInt() =>
        Product is Consumable || (Product as Substance)!.PriceUnit == MeasurementUnits.Piece;

    private bool IsError =>
        Amount <= 0.0 || AmountInt <= 0 || (Product is Consumable c && AmountInt > c.Amount);

    // Simple Functions to Submit and Cancel the Action.
    private void Submit()
    {
        if (IsInt())
        {
            ProductDialog.Close(DialogResult.Ok((double)AmountInt));
        }
        else
        {
            ProductDialog.Close(DialogResult.Ok(Amount));
        }
    }

    private void Cancel() => ProductDialog.Cancel();

    /// <summary>
    /// Check if Amount is higher than 0.
    /// false: dialog will not be closed.
    /// true: submit will be successful.
    /// </summary>
    private void CheckAndSubmit()
    {
        if (Amount > 0 && AmountInt > 0)
        {
            if (Product is Consumable c)
            {
                if (AmountInt > c.Amount)
                {
                    _logger.LogWarning("Consumable is out of Stock");
                    return;
                }
            }
            _logger.LogInformation("Product added with quantity: {D}", Amount);
            Submit();
        }
        else
        {
            _logger.LogWarning("Quantity must be greater than 0");
        }
    }
}
