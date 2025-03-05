using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Extensions;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.Pages;

[Route("/shop/{WorkingGroupId:guid}/{PurchaseId:guid}")]
public class InvalidatePurchasePage : ShoppingCartPage
{
    [Parameter]
    public Guid WorkingGroupId { get; set; }

    [Parameter]
    public Guid PurchaseId { get; set; }

    private readonly IDialogService _dialogService;
    private readonly IPurchaseApi _purchaseApi;
    private readonly ISnackbar _snackbar;
    private readonly NavigationManager _navigationManager;

    /// <summary>
    /// This saves the state of touched consumables.
    /// </summary>
    private readonly Dictionary<Guid, int> _touchedConsumables = [];

    public InvalidatePurchasePage(
        IDialogService dialogService,
        IPurchaseApi purchaseApi,
        ISnackbar snackbar,
        NavigationManager navigationManager
    )
        : base(dialogService, purchaseApi, snackbar)
    {
        _dialogService = dialogService;
        _purchaseApi = purchaseApi;
        _snackbar = snackbar;
        _navigationManager = navigationManager;
    }

    protected override async Task OnInitializedAsync()
    {
        var purchase = await _purchaseApi.GetAsync(WorkingGroupId, PurchaseId);
        purchase.Switch(
            val => Purchase = val,
            err =>
                throw new InvalidOperationException(
                    $"Could not retrieve Purchase {PurchaseId} of WorkingGroup {WorkingGroupId}. Reason: {string.Join(",", err.Select(e => e.Description))}"
                )
        );
    }

    protected override async Task OpenSubmitDialogAsync()
    {
        bool? invalidate = await _dialogService.ShowMessageBox(
            new MessageBoxOptions
            {
                Title = "Kauf invalidieren",
                Message = "Möchten sie den kauf invalidieren und den aktualisierten speichern?",
                YesText = "Invalidieren",
                CancelText = "Abbrechen",
            }
        );

        if (invalidate is not null && invalidate.Value)
        {
            byte[]? signingResult = await OpenSigningDialog();

            if (signingResult is null)
                return;

            var success = await _purchaseApi.InvalidateAsync(
                WorkingGroupId,
                PurchaseId,
                Purchase.ToCreateDto() with
                {
                    Signature = signingResult,
                }
            );

            if (success.IsError)
            {
                _snackbar.Add("Etwas ist beim updaten schiefgelaufen", Severity.Error);
            }
            else
            {
                _snackbar.Add("Erfolgreich aktualisiert", Severity.Success);
            }

            _navigationManager.NavigateTo("/");
        }
    }

    protected override void AdaptAmountOfConsumableIfNeeded(AssetItem assetItem)
    {
        if (
            assetItem is Consumable consumable
            && Purchase.Entries.All(entry => entry.AssetItem.Id != consumable.Id)
            && _touchedConsumables.Any(pair => pair.Key == consumable.Id)
        )
        {
            consumable.Amount += _touchedConsumables[consumable.Id];
        }
        else
        {
            base.AdaptAmountOfConsumableIfNeeded(assetItem);
        }
    }

    protected override void AdaptAmountOfAllConsumablesByAssetItemId(
        Consumable consumable,
        int amount
    )
    {
        base.AdaptAmountOfAllConsumablesByAssetItemId(consumable, amount);
        if (_touchedConsumables.Any(pair => pair.Key == consumable.Id))
        {
            _touchedConsumables[consumable.Id] -= amount;
        }
        else
        {
            _touchedConsumables.Add(consumable.Id, consumable.Amount);
        }
    }
}
