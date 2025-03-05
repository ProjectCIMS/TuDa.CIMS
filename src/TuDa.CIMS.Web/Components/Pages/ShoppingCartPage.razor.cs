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

    /// <summary>
    /// This saves the state of touched consumables.
    /// </summary>
    private readonly Dictionary<Guid, int> _touchedConsumables = [];

    protected PurchaseResponseDto Purchase { get; set; } = new() { Buyer = null! };

    private async Task OpenSelectDialogAsync(AssetItem product)
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };

        var consumable = AdaptAmountOfConsumableIfNeeded(product);

        // Set Parameters
        var parameters = new DialogParameters { { "Product", consumable ?? product } };

        var dialog = await _dialogService.ShowAsync<ShoppingCartProductDialog>(
            "Mengenangabe",
            parameters,
            options
        );

        double? amount = await dialog.GetReturnValueAsync<double?>();
        if (amount is > 0)
        {
            AddProductEntry(amount.Value, product);
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

        var ids = await dialog.GetReturnValueAsync<WorkingGroupWithBuyer>();

        if (ids is null)
            return;

        byte[]? signResult = await OpenSigningDialog();

        if (signResult is null)
            return;

        var errorOr = await _purchaseApi.CreateAsync(
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

        return await signDialog.GetReturnValueAsync<byte[]?>();
    }

    private void ResetEntries()
    {
        Purchase.Entries.Clear();
    }

    private void AddProductEntry(double amount, AssetItem product)
    {
        Purchase.Entries.Add(
            new PurchaseEntry()
            {
                Amount = amount,
                AssetItem = product,
                PricePerItem = product.Price,
            }
        );
        if (product is Consumable consumable)
        {
            AdaptAmountOfAllConsumablesByAssetItemId(consumable, (int)amount);
        }
    }

    private void RemovePurchaseEntry(PurchaseEntry entry)
    {
        if (entry.AssetItem is Consumable consumable)
        {
            AdaptAmountOfAllConsumablesByAssetItemId(consumable, -(int)entry.Amount);
        }
        Purchase.Entries.Remove(entry);
        StateHasChanged();
    }

    /// <summary>
    /// Updates the amount of a consumable if it is already in the purchase or was touched.
    /// </summary>
    /// <param name="assetItem">The asset item to check and update.</param>
    /// <returns>Adapted consumable if it was touched, otherwise null.</returns>
    private Consumable? AdaptAmountOfConsumableIfNeeded(AssetItem assetItem)
    {
        if (
            assetItem is not Consumable consumable
            || _touchedConsumables.All(pair => pair.Key != consumable.Id)
        )
        {
            return null;
        }

        return consumable with
        {
            Amount = _touchedConsumables[consumable.Id],
        };
    }

    /// <summary>
    /// Adapts the amount of all consumables with the same asset item id.
    /// </summary>
    /// <param name="consumable">The consumable to adapt.</param>
    /// <param name="amountToSubtract">The amount to subtract.</param>
    private void AdaptAmountOfAllConsumablesByAssetItemId(
        Consumable consumable,
        int amountToSubtract
    )
    {
        if (_touchedConsumables.Any(pair => pair.Key == consumable.Id))
        {
            _touchedConsumables[consumable.Id] -= amountToSubtract;
        }
        else
        {
            _touchedConsumables.Add(consumable.Id, consumable.Amount - amountToSubtract);
        }
    }
}
