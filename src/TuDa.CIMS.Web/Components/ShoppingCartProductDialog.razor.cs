using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components;

public partial class ShoppingCartProductDialog
{
    [CascadingParameter]
    public required MudDialogInstance ProductDialog { get; set; }

    [Inject]
    public required ILogger<ShoppingCartProductDialog> Logger { get; set; }

    // Product to be shown in Dialog.
    [Parameter]
    public required AssetItem Product { get; set; }

    // Amount of Product
    public uint Amount { get; set; } = 1;
    private bool isError => Amount <= 0;

    // Simple Functions to Submit and Cancel the Action.
    private void Submit() => ProductDialog.Close(DialogResult.Ok(Amount));

    private void Cancel() => ProductDialog.Cancel();

    /// <summary>
    /// Method to add Product to Product List.
    /// </summary>
    private void AddProduct()
    {
        // Logic to add the product goes here.
        // For now, just close the dialog after clicking "Add".
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
