using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Components.ShoppingCart;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class ShoppingCartPage
{
    [Inject]
    public required IDialogService DialogService { get; set; }

    private PurchaseEntry? PurchaseEntry { get; set; }
    private static List<PurchaseEntry> Entries { get; set; } = [];

    private Purchase Purchase { get; set; } =
        new Purchase
        {
            Entries = Entries,
            Buyer = new Student { FirstName = "John", Name = "John Doe" },
        };

    public required AssetItem Product { get; set; }

    private async Task OpenDialogAsync(AssetItem assetItem)
    {
        Product = assetItem;

        var options = new DialogOptions { CloseOnEscapeKey = true };

        // Set Parameters
        var parameters = new DialogParameters { { "Product", assetItem } };

        var dialog = await DialogService.ShowAsync<ShoppingCartProductDialog>(
            "Amount Input",
            parameters,
            options
        );
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            int? amount = result.Data as int?;
            if (amount > 0)
            {
                AddProductEntry(amount.Value);
            }
        }
    }

    // TODO: Cannot access a disposed object error
    private async Task OpenSubmitDialogAsync()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        var parameters = new DialogParameters
        {
            { "Entries", Entries },
            { "Working Groups", new List<WorkingGroup>() },
        };

        /*var dialog = await DialogService.ShowAsync<ShoppingCartSubmitPopup>(
            "Submit Input",
            parameters,
            options
        );*/
    }

    private void AddProductEntry(int amount)
    {
        PurchaseEntry = new PurchaseEntry()
        {
            Amount = amount,
            AssetItem = Product,
            PricePerItem = Product.Price,
        };

        Entries.Add(PurchaseEntry);
    }
}
