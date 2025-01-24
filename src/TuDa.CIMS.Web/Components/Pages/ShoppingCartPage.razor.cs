using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Web.Components.ShoppingCart;
using TuDa.CIMS.Web.Helper;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class ShoppingCartPage
{
    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    [Inject]
    private IPurchaseApi PurchaseApi { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

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
        var parameters = new DialogParameters { { "PurchaseEntries", Purchase.Entries } };

        var dialog = await DialogService.ShowAsync<ShoppingCartSubmitPopup>(
            "Einkauf Bestätigen",
            parameters,
            options
        );

        if ((await dialog.Result).Canceled)
            return;

        //TODO: Send Working Group, Buyer and Purchase to API
        var ids = await dialog.GetReturnValueAsync<WorkingGroupWithBuyer>();

        if (ids is null)
            Snackbar.Add("Beim abschließen ist etwas schiefgelaufen", Severity.Error);

        var errorOr = await PurchaseApi.CreateAsync(
            ids.WorkingGroupId,
            new CreatePurchaseDto
            {
                Buyer = ids.BuyerId,
                Entries = Purchase
                    .Entries.Select(entry => new CreatePurchaseEntryDto
                    {
                        AssetItemId = entry.AssetItem.Id,
                        Amount = entry.Amount,
                        PricePerItem = entry.PricePerItem,
                    })
                    .ToList(),
                CompletionDate = DateTime.Now.ToUniversalTime(),
            }
        );

        if (errorOr.IsError)
        {
            Snackbar.Add("Beim abschließen ist etwas schiefgelaufen", Severity.Error);
        }
        else
        {
            Snackbar.Add("Kauf erfolgreich abgeschlossen", Severity.Success);
            ResetEntries();
            StateHasChanged();
        }
    }

    private void ResetEntries()
    {
        Purchase.Entries.Clear();
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
