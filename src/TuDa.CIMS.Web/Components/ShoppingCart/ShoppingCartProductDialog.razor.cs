using System.Globalization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Web.Components.ShoppingCart;

public partial class ShoppingCartProductDialog
{
    [CascadingParameter]
    public required MudDialogInstance ProductDialog { get; set; }

    [Inject]
    private ILogger<ShoppingCartProductDialog> Logger { get; set; } = null!;

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
    public bool IsInt() => Product is Consumable;

    private bool IsError => Amount <= 0.0 || AmountInt <= 0;

    // Simple Functions to Submit and Cancel the Action.
    private void Submit()
    {
        if (IsInt())
        {
            ProductDialog.Close(DialogResult.Ok(AmountInt));
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
        if (Amount > 0)
        {
            Logger.LogInformation($"Product added with quantity: {Amount}");
            Submit();
        }
        else
        {
            Logger.LogWarning("Quantity must be greater than 0");
        }
    }
}
