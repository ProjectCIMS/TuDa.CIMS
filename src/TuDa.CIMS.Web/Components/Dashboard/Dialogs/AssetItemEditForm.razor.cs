using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.Dashboard.Dialogs;

public partial class AssetItemEditForm
{
    private readonly IAssetItemApi _assetItemApi;
    private readonly ISnackbar _snackbar;

    public AssetItemEditForm(IAssetItemApi assetItemApi, ISnackbar snackbar)
    {
        _assetItemApi = assetItemApi;
        _snackbar = snackbar;
    }

    /// <summary>
    /// Fields for Errors and Feedback
    /// </summary>
    public bool ShowError = false;

    private string FeedbackMessage = string.Empty;
    private Severity FeedbackColor = Severity.Success;

    /// <summary>
    /// References to the different Forms
    /// </summary>
    private AssetItemForm _assetItemForm;
    private ChemicalItemForm _chemicalItemForm;
    private ConsumableItemForm _consumableItemForm;
    private GasCylinderItemForm _gasCylinderForm;

    [Parameter]
    public required AssetItem UpdateItem { get; set; }

    public async Task DeleteItem()
    {
        await _assetItemApi.RemoveAsync(UpdateItem.Id);
    }

    /// <summary>
    /// Resetting all Inputs on every Form
    /// </summary>
    private void ResetInputs()
    {
        switch (UpdateItem)
        {
            case Chemical:
            {
                _chemicalItemForm.ResetInputs();
                break;
            }

            case Consumable:
            {
                _consumableItemForm.ResetInputs();
                break;
            }

            case GasCylinder:
            {
                _gasCylinderForm.ResetInputs();
                break;
            }
        }

        ShowError = false;
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender)
            return;

        AssetItemEditFormLoad();
        StateHasChanged();
    }

    /// <summary>
    /// fill the input fields with the Item
    /// </summary>
    public void AssetItemEditFormLoad()
    {
        _assetItemForm.SetForm(UpdateItem);

        switch (UpdateItem)
        {
            case Chemical:
            {
                if (UpdateItem is Chemical chemical)
                {
                    _chemicalItemForm.SetForm(chemical);
                }

                break;
            }

            case Consumable:
            {
                if (UpdateItem is Consumable consumable)
                {
                    _consumableItemForm.SetForm(consumable);
                }

                break;
            }

            case GasCylinder:
            {
                if (UpdateItem is GasCylinder gasCylinder)
                {
                    _gasCylinderForm.SetForm(gasCylinder);
                }

                break;
            }
        }
    }

    /// <summary>
    /// Check if all required Inputs are done
    /// </summary>
    /// <returns></returns>
    public bool ValidateForm()
    {
        if (
            _assetItemForm.ValidateForm()
            || (
                (
                    UpdateItem.GetType() == typeof(Chemical)
                    || UpdateItem.GetType() == typeof(Solvent)
                ) && _chemicalItemForm.ValidateForm()
            )
            || (UpdateItem.GetType() == typeof(Consumable) && _consumableItemForm.ValidateForm())
            || (UpdateItem.GetType() == typeof(GasCylinder) && _gasCylinderForm.ValidateForm())
        )
        {
            ResetInputs();
            ShowError = true;
            OnValidation.InvokeAsync();
            return true;
        }

        OnValidation.InvokeAsync();
        return false;
    }

    /// <summary>
    /// Happens when the Forms are getting validated
    /// </summary>
    [Parameter]
    public EventCallback OnValidation { get; set; }

    /// <summary>
    /// Functionality of the "Änderungen speichern" Button: Bind the Values to the actual Item
    /// </summary>
    public async Task SaveChanges()
    {
        UpdateAssetItemDto? dto = UpdateItem switch
        {
            Chemical => new UpdateChemicalDto()
            {
                Name = _assetItemForm.FormName,
                Shop = _assetItemForm.FormShop,
                ItemNumber = _assetItemForm.FormItemNumber,
                Note = _assetItemForm.FormNote,
                Cas = _chemicalItemForm.FormCas,
                Price = _assetItemForm.FormPrice,
                Purity = _chemicalItemForm.FormPurity,
                PriceUnit = _chemicalItemForm.FormPriceUnit,
            },

            Consumable => new UpdateConsumableDto()
            {
                Name = _assetItemForm.FormName,
                Shop = _assetItemForm.FormShop,
                ItemNumber = _assetItemForm.FormItemNumber,
                Note = _assetItemForm.FormNote,
                Price = _assetItemForm.FormPrice,
                Manufacturer = _consumableItemForm.FormManufacturer,
                SerialNumber = _consumableItemForm.FormSerialNumber,
                Amount = _consumableItemForm.FormConsumableAmount,
            },

            GasCylinder => new UpdateGasCylinderDto()
            {
                Name = _assetItemForm.FormName,
                Shop = _assetItemForm.FormShop,
                ItemNumber = _assetItemForm.FormItemNumber,
                Note = _assetItemForm.FormNote,
                Price = _assetItemForm.FormPrice,
                Cas = _chemicalItemForm.FormCas,
                Purity = _chemicalItemForm.FormPurity,
                Volume = _gasCylinderForm.FormVolume,
                Pressure = _gasCylinderForm.FormPressure,
                PriceUnit = _chemicalItemForm.FormPriceUnit,
            },
            _ => null!,
        };
        FeedbackMessage = "Das Objekt wurde erfolgreich geupdated.";
        FeedbackColor = Severity.Success;

        await _assetItemApi.UpdateAsync(UpdateItem.Id, dto);
        _snackbar.Add(FeedbackMessage, FeedbackColor);
    }
}
