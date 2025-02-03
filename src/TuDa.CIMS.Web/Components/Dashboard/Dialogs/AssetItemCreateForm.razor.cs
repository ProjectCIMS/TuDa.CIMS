using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.Dashboard.Dialogs;

public partial class AssetItemCreateForm
{
    private readonly IAssetItemApi _assetItemApi;
    private readonly ISnackbar _snackbar;

    public AssetItemCreateForm(IAssetItemApi assetItemApi, ISnackbar snackbar)
    {
        _assetItemApi = assetItemApi;
        _snackbar = snackbar;
    }

    /// <summary>
    /// Fields for Errors and Feedback
    /// </summary>
    private bool _showError = false;
    private bool _showChemicalError = false;
    private bool _showConsumableError = false;
    private bool _showGasCylinderError = false;
    private bool _showSolventError = false;

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
                break;
            }

            case AssetItemType.Solvent:
            {
                _chemicalItemForm.ResetInputs();
                break;
            }
        }
        _showError = false;
        _showChemicalError = false;
        _showConsumableError = false;
        _showGasCylinderError = false;
        _showSolventError = false;
    }

    /// <summary>
    /// Check if all required Inputs are done
    /// </summary>
    /// <returns>returns true if an error exist in any form</returns>
    public bool ErrorsInForm()
    {
        // Überprüfe nur die Form, die dem aktuellen AssetItemType entspricht
        bool hasErrors = false;

        switch (_selectedAssetItemType)
        {
            case AssetItemType.Chemical:
                hasErrors = _chemicalItemForm.ErrorsInForm() || _assetItemForm.ErrorsInForm();
                _showChemicalError = hasErrors;
                break;
            case AssetItemType.Solvent:
                hasErrors = _chemicalItemForm.ErrorsInForm() || _assetItemForm.ErrorsInForm();
                _showSolventError = hasErrors;
                break;

            case AssetItemType.Consumable:
                hasErrors = _consumableItemForm.ErrorsInForm() || _assetItemForm.ErrorsInForm();
                _showConsumableError = hasErrors;
                break;

            case AssetItemType.GasCylinder:
                hasErrors = _gasCylinderForm.ErrorsInForm() || _assetItemForm.ErrorsInForm();
                _showGasCylinderError = hasErrors;
                break;

            default:
                hasErrors = _assetItemForm.ErrorsInForm();
                _showError = hasErrors;
                break;
        }
        if (hasErrors)
        {
            _showError = true;
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
    /// Functionality of the "Änderungen speichern" Button: Create the Item
    /// </summary>
    public async Task<CreateAssetItemDto> SaveChanges()
    {
        var errorOrItems = await _assetItemApi.GetAllAsync();
        var items = errorOrItems.Value.ToList();
        Guid RoomId = items.FirstOrDefault().Room.Id;

        CreateAssetItemDto? createDto = _selectedAssetItemType switch
        {
            AssetItemType.Chemical => new CreateChemicalDto
            {
                Name = _assetItemForm.FormName,
                Shop = _assetItemForm.FormShop,
                ItemNumber = _assetItemForm.FormItemNumber,
                Note = _assetItemForm.FormNote,
                Cas = _chemicalItemForm.FormCas,
                Price = _assetItemForm.FormPrice,
                Purity = _chemicalItemForm.FormPurity,
                PriceUnit = _chemicalItemForm.FormPriceUnit!.Value,
                BindingSize = _chemicalItemForm.FormBindingSize,
                RoomId = RoomId,
            },

            AssetItemType.Consumable => new CreateConsumableDto
            {
                Name = _assetItemForm.FormName,
                Shop = _assetItemForm.FormShop,
                ItemNumber = _assetItemForm.FormItemNumber,
                Note = _assetItemForm.FormNote,
                Price = _assetItemForm.FormPrice,
                RoomId = RoomId,
                Manufacturer = _consumableItemForm.FormManufacturer,
                SerialNumber = _consumableItemForm.FormSerialNumber,
                Amount = _consumableItemForm.FormConsumableAmount,
            },

            AssetItemType.GasCylinder => new CreateGasCylinderDto
            {
                Name = _assetItemForm.FormName,
                Shop = _assetItemForm.FormShop,
                ItemNumber = _assetItemForm.FormItemNumber,
                Note = _assetItemForm.FormNote,
                Price = _assetItemForm.FormPrice,
                RoomId = RoomId,
                Cas = _gasCylinderForm.FormCas,
                Purity = _gasCylinderForm.FormPurity,
                Volume = _gasCylinderForm.FormVolume,
                Pressure = _gasCylinderForm.FormPressure,
                PriceUnit = _gasCylinderForm.FormPriceUnit!.Value,
            },

            AssetItemType.Solvent => new CreateSolventDto
            {
                Name = _assetItemForm.FormName,
                Shop = _assetItemForm.FormShop,
                ItemNumber = _assetItemForm.FormItemNumber,
                Note = _assetItemForm.FormNote,
                Cas = _chemicalItemForm.FormCas,
                Price = _assetItemForm.FormPrice,
                Purity = _chemicalItemForm.FormPurity,
                PriceUnit = _chemicalItemForm.FormPriceUnit!.Value,
                BindingSize = _chemicalItemForm.FormBindingSize,
                RoomId = RoomId,
            },

            _ => null, // Should never happen!
        };
        _snackbar.Add("Das Objekt wurde erfolgreich erstellt.", Severity.Success);
        return createDto!;
    }
}
