using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Web.Components.ShoppingCart;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class ShoppingCartPage
{
    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    private Purchase Purchase { get; set; } = new() { Buyer = null! };

    private async Task OpenDialogAsync(AssetItem product)
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };

        // Set Parameters
        var parameters = new DialogParameters { { "Product", product } };

        var dialog = await DialogService.ShowAsync<ShoppingCartProductDialog>(
            "Mengenangabe",
            parameters,
            options
        );
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            int amount = (int)result.Data!;
            if (amount > 0)
            {
                AddProductEntry(amount, product);
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
                // TODO: remove it when Working Groups are finished
                "WorkingGroups",
                new List<WorkingGroup>()
                {
                    new()
                    {
                        Professor = new Professor
                        {
                            Name = "Heiter",
                            Address = null,
                            Gender = Gender.Unknown,
                        },
                    },
                    new()
                    {
                        Professor = new Professor
                        {
                            Name = "Kaiser",
                            Address = null,
                            Gender = Gender.Unknown,
                        },
                    },
                }
            },
        };

        var dialog = await DialogService.ShowAsync<ShoppingCartSubmitPopup>(
            "Einkauf Bestätigen",
            parameters,
            options
        );

        // TODO: Send Working Group, Buyer and Purchase to API
        // var workingGroup = await dialog.GetReturnValueAsync<WorkingGroup>();
        // if (await dialog.GetReturnValueAsync<WorkingGroup>() is not null){}
    }

    private void AddProductEntry(int amount, AssetItem product)
    {
        Purchase.Entries.Add(
            new PurchaseEntry()
            {
                Amount = amount,
                AssetItem = product,
                PricePerItem = product.Price,
            }
        );
    }
}
