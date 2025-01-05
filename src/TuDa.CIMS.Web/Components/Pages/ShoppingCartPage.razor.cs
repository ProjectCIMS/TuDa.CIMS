﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Components.ShoppingCart;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class ShoppingCartPage
{
    [Inject] private IDialogService DialogService { get; set; } = null!;

    private Purchase Purchase { get; set; } = null!;

    public required AssetItem Product { get; set; }

    private async Task OpenDialogAsync(AssetItem assetItem)
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };

        // Set Parameters
        var parameters = new DialogParameters { { "Product", assetItem } };

        var dialog = await DialogService.ShowAsync<ShoppingCartProductDialog>(
            "Mengenangabe",
            parameters,
            options
        );
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            int amount = (int) result.Data!;
            if (amount > 0)
            {
                AddProductEntry(amount, assetItem);
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
                    new() { Professor = new Professor { Name = "Heiter" } },
                    new() { Professor = new Professor { Name = "Kaiser" } },
                }
            },
        };

        var dialog = await DialogService.ShowAsync<ShoppingCartSubmitPopup>(
            "Einkauf Bestätigen",
            parameters,
            options
        );

        var workingGroup = await dialog.GetReturnValueAsync<WorkingGroup>();
        if (await dialog.GetReturnValueAsync<WorkingGroup>() is not null)
        {
           //Purchase does not have a working group attribute
        }
    }

    private void AddProductEntry(int amount, AssetItem assetItem)
    {
        Purchase.Entries.Add(
            new PurchaseEntry()
            {
                Amount = amount,
                AssetItem = assetItem,
                PricePerItem = Product.Price,
            }
        );
    }
}
