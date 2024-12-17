using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Components.ShoppingCart;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class ShoppingCartPage
{
    [Inject]
    public required IDialogService DialogService { get; set; }

    private Purchase Purchase { get; set; } =
        new()
        {
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

    private async Task OpenSubmitDialogAsync()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        var parameters = new DialogParameters
        {
            { "PurchaseEntries", Purchase.Entries },
            {
                "WorkingGroups",
                new List<WorkingGroup>()
                {
                    new() { Professor = new Professor { Name = "Heiter" } },
                    new() { Professor = new Professor { Name = "Kaiser" } },
                }
            },
        };

        var dialog = await DialogService.ShowAsync<ShoppingCartSubmitPopup>(
            "Submit Input",
            parameters,
            options
        );

        if (await dialog.GetReturnValueAsync<WorkingGroup>() is not null)
        {
            Purchase = new Purchase { Buyer = new Student { Name = "Jon" } };
        }
    }

    private void AddProductEntry(int amount)
    {
        Purchase.Entries.Add(
            new PurchaseEntry()
            {
                Amount = amount,
                AssetItem = Product,
                PricePerItem = Product.Price,
            }
        );
    }
}
