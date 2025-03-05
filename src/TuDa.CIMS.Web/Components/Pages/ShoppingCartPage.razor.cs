using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Components.ShoppingCart;
using TuDa.CIMS.Web.Helper;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class ShoppingCartPage
{
    private readonly IDialogService _dialogService;
    private readonly IPurchaseApi _purchaseApi;
    private readonly ISnackbar _snackbar;

    public ShoppingCartPage(
        IDialogService dialogService,
        IPurchaseApi purchaseApi,
        ISnackbar snackbar
    )
    {
        _dialogService = dialogService;
        _purchaseApi = purchaseApi;
        _snackbar = snackbar;
    }

    protected PurchaseResponseDto Purchase { get; set; } = new() { Buyer = null! };

    private async Task OpenSelectDialogAsync(AssetItem product)
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };

        AdaptAmountOfConsumableIfNeeded(product);

        // Set Parameters
        var parameters = new DialogParameters { { "Product", product } };

        var dialog = await _dialogService.ShowAsync<ShoppingCartProductDialog>(
            "Mengenangabe",
            parameters,
            options
        );
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            double amount = (double)result.Data!;
            if (amount > 0)
            {
                AddProductEntry(amount, product);
            }
        }
    }

    protected virtual async Task OpenSubmitDialogAsync()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        var parameters = new DialogParameters { { "PurchaseEntries", Purchase.Entries } };

        var dialog = await _dialogService.ShowAsync<ShoppingCartSubmitPopup>(
            "Einkauf Bestätigen",
            parameters,
            options
        );
        var result = await dialog.Result;

        if (result is { Canceled: true })
            return;

        var ids = await dialog.GetReturnValueAsync<WorkingGroupWithBuyer>();

        if (ids is null)
            _snackbar.Add("Beim abschließen ist etwas schiefgelaufen", Severity.Error);

        byte[]? signResult = await OpenSigningDialog();

        if (signResult is null)
            return;

        var errorOr = await _purchaseApi.CreateAsync(
            ids!.WorkingGroupId,
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
                Signature = signResult,
            }
        );

        if (errorOr.IsError)
        {
            _snackbar.Add("Beim abschließen ist etwas schiefgelaufen", Severity.Error);
        }
        else
        {
            _snackbar.Add("Kauf erfolgreich abgeschlossen", Severity.Success);
            ResetEntries();
            StateHasChanged();
        }
    }

    protected async Task<byte[]?> OpenSigningDialog()
    {
        var signOptions = new DialogOptions
        {
            CloseOnEscapeKey = true,
            BackdropClick = false,
            FullWidth = true,
            MaxWidth = MaxWidth.Large,
        };
        var signDialog = await _dialogService.ShowAsync<SignDialog>(
            "Unterschrift erforderlich",
            signOptions
        );

        var signResult = await signDialog.Result;

        if (signResult?.Canceled ?? false)
        {
            _snackbar.Add("Unterschrift wurde abgebrochen", Severity.Warning);
            return null;
        }

        return signResult?.Data as byte[];
    }

    private void AdaptAmountOfConsumableIfNeeded(AssetItem assetItem)
    {
        if (assetItem is not Consumable consumable)
            return;

        var entry = Purchase.Entries.FirstOrDefault(entry => entry.AssetItem.Id == assetItem.Id);
        if (entry?.AssetItem is not Consumable sameConsumable)
            return;

        consumable.Amount = sameConsumable.Amount;
    }

    private void ResetEntries()
    {
        Purchase.Entries.Clear();
    }

    private void AddProductEntry(double amount, AssetItem product)
    {
        if (product is Consumable consumable)
        {
            product = AdaptAmountOfAllConsumablesByAssetItemId(consumable, (int)amount);
        }

        Purchase.Entries.Add(
            new PurchaseEntry()
            {
                Amount = amount,
                AssetItem = product,
                PricePerItem = product.Price,
            }
        );
    }

    private void RemovePurchaseEntry(PurchaseEntry entry)
    {
        if (entry.AssetItem is Consumable consumable)
        {
            AdaptAmountOfAllConsumablesByAssetItemId(consumable, (int)entry.Amount);
        }
        Purchase.Entries.Remove(entry);
        StateHasChanged();
    }

    private Consumable AdaptAmountOfAllConsumablesByAssetItemId(Consumable consumable, int amount)
    {
        var sameConsumables = Purchase
            .Entries.Where(entry => entry.AssetItem.Id == consumable.Id)
            .Select(entry => entry.AssetItem as Consumable!);

        foreach (var c in sameConsumables)
        {
            c!.Amount -= amount;
        }

        return consumable with
        {
            Amount = consumable.Amount - amount,
        };
    }
}
