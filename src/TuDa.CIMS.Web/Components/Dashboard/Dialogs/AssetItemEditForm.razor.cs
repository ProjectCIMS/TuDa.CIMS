using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Web.Components.Dashboard.Dialogs;

public partial class AssetItemEditForm
{
    /// <summary>
    /// Fields for Errors and Feedback
    /// </summary>
    public bool ShowError = false;

    /// <summary>
    /// References to the different Forms
    /// </summary>
    private AssetItemForm _assetItemForm = null!;
    private ChemicalItemForm _chemicalItemForm = null!;
    private ConsumableItemForm _consumableItemForm = null!;
    private GasCylinderItemForm _gasCylinderForm = null!;

    [Parameter]
    public required AssetItem UpdateItem { get; set; }

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
    public bool ErrorsInForm()
    {
        if (
            _assetItemForm.ErrorsInForm()
            || (
                (
                    UpdateItem.GetType() == typeof(Chemical)
                    || UpdateItem.GetType() == typeof(Solvent)
                ) && _chemicalItemForm.ErrorsInForm()
            )
            || (UpdateItem.GetType() == typeof(Consumable) && _consumableItemForm.ErrorsInForm())
            || (UpdateItem.GetType() == typeof(GasCylinder) && _gasCylinderForm.ErrorsInForm())
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
    public UpdateAssetItemDto SaveChanges()
    {
        UpdateAssetItemDto dto = UpdateItem switch
        {
            Chemical => new UpdateChemicalDto()
            {
                Name = _assetItemForm.FormName,
                Shop = _assetItemForm.FormShop,
                ItemNumber = _assetItemForm.FormItemNumber,
                Note = _assetItemForm.FormNote,
                Room = _assetItemForm.FormRoom,
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
                Room = _assetItemForm.FormRoom,
                Manufacturer = _consumableItemForm.FormManufacturer,
                SerialNumber = _consumableItemForm.FormSerialNumber,
                StockUpdate = new StockUpdateDto(
                    _consumableItemForm.FormConsumableAmount,
                    TransactionReasons.Restock
                ),
            },

            GasCylinder => new UpdateGasCylinderDto()
            {
                Name = _assetItemForm.FormName,
                Shop = _assetItemForm.FormShop,
                ItemNumber = _assetItemForm.FormItemNumber,
                Note = _assetItemForm.FormNote,
                Price = _assetItemForm.FormPrice,
                Room = _assetItemForm.FormRoom,
                Cas = _chemicalItemForm.FormCas,
                Purity = _chemicalItemForm.FormPurity,
                Volume = _gasCylinderForm.FormVolume,
                Pressure = _gasCylinderForm.FormPressure,
                PriceUnit = _chemicalItemForm.FormPriceUnit,
            },
            _ => null!,
        };
        return dto;
    }
}
