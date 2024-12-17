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

    public PurchaseEntry PurchaseEntry { get; set; }
    public static List<PurchaseEntry> Entries { get; set; } = [];

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
        if (Amount > 0)
        {
            Logger.LogInformation($"Product added with quantity: {Amount}");

            PurchaseEntry = new PurchaseEntry()
            {
                Amount = Amount,
                AssetItem = Product,
                PricePerItem = Product.Price,
            };

            Entries.Add(PurchaseEntry);
            Submit();
        }
        else
        {
            Logger.LogWarning("Quantity must be greater than 0");
        }
    }
}
