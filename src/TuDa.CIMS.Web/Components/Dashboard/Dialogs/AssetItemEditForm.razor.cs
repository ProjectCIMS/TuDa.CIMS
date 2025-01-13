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
    /// To choose from the different forms for inputting
    /// </summary>
    private AssetItemType _selectedAssetItemType = AssetItemType.Chemical;

    /// <summary>
    /// References to the different Forms
    /// </summary>
    private AssetItemForm _assetItemForm;
    private ChemicalItemForm _chemicalItemForm;
    private ConsumableItemForm _consumableItemForm;
    private GasCylinderItemForm _gasCylinderForm;

    [Parameter]
    public required AssetItem UpdateItem { get; set; }

    /// <summary>
    /// Resetting all Inputs on every Form
    /// </summary>
    private void ResetInputs()
    {
        switch (_selectedAssetItemType)
        {
            case AssetItemType.Chemical:
            {
                _chemicalItemForm.ResetInputs();
                break;
            }

            case AssetItemType.Consumable:
            {
                _consumableItemForm.ResetInputs();
                break;
            }

            case AssetItemType.GasCylinder:
            {
                _gasCylinderForm.ResetInputs();
                _chemicalItemForm.ResetInputs();
                break;
            }

            case AssetItemType.Solvent:
            {
                _chemicalItemForm.ResetInputs();
                break;
            }
        }
        ShowError = false;
    }

    /// <summary>
    /// fill the input fields with the Item
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _assetItemForm.SetForm(UpdateItem);

            switch (_selectedAssetItemType)
            {
                case AssetItemType.Chemical:
                {
                    if (UpdateItem is Chemical chemical)
                    {
                        _chemicalItemForm.SetForm(chemical);
                    }
                    break;
                }

                case AssetItemType.Consumable:
                {
                    if (UpdateItem is Consumable consumable)
                    {
                        _consumableItemForm.SetForm(consumable);
                    }

                    break;
                }

                case AssetItemType.GasCylinder:
                {
                    if (UpdateItem is GasCylinder gasCylinder)
                    {
                        _gasCylinderForm.SetForm(gasCylinder);
                        _chemicalItemForm.FormCas = gasCylinder.Cas;
                        _chemicalItemForm.FormPurity = gasCylinder.Purity;
                        _chemicalItemForm.FormPriceUnit = gasCylinder.PriceUnit;
                    }
                    break;
                }

                case AssetItemType.Solvent:
                {
                    if (UpdateItem is Solvent solvent)
                    {
                        _chemicalItemForm.SetForm(solvent);
                    }
                    break;
                }
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
                    _selectedAssetItemType == AssetItemType.Chemical
                    || _selectedAssetItemType == AssetItemType.Solvent
                ) && _chemicalItemForm.ValidateForm()
            )
            || (
                _selectedAssetItemType == AssetItemType.Consumable
                && _consumableItemForm.ValidateForm()
            )
            || (
                _selectedAssetItemType == AssetItemType.GasCylinder
                && (_gasCylinderForm.ValidateForm() || _chemicalItemForm.ValidateForm())
            )
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
        UpdateAssetItemDto? dto = _selectedAssetItemType switch
        {
            AssetItemType.Chemical => new UpdateChemicalDto()
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

            AssetItemType.Consumable => new UpdateConsumableDto()
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

            AssetItemType.GasCylinder => new UpdateGasCylinderDto()
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

            AssetItemType.Solvent => new UpdateSolventDto()
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
            _ => null!,
        };
        FeedbackMessage = "Das Objekt wurde erfolgreich geupdated.";
        FeedbackColor = Severity.Success;

        await _assetItemApi.UpdateAsync(UpdateItem.Id, dto);
        _snackbar.Add(FeedbackMessage, FeedbackColor);
    }
}
