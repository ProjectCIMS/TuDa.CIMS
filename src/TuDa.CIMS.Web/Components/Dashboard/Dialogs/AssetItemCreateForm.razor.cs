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
    public bool ShowError = false;

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
        ShowError = false;
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
                && _gasCylinderForm.ValidateForm()
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
        string feedbackMessage;
        Severity feedbackColor;
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
                PriceUnit = _chemicalItemForm.FormPriceUnit,
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
                Cas = _chemicalItemForm.FormCas,
                Purity = _chemicalItemForm.FormPurity,
                Volume = _gasCylinderForm.FormVolume,
                Pressure = _gasCylinderForm.FormPressure,
                PriceUnit = _chemicalItemForm.FormPriceUnit,
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
                PriceUnit = _chemicalItemForm.FormPriceUnit,
                RoomId = RoomId,
            },

            _ => null,
        };

        if (createDto != null)
        {
            try
            {
                await _assetItemApi.CreateAsync(createDto);
                feedbackMessage = "Das Objekt wurde erfolgreich erstellt.";
                feedbackColor = Severity.Success;
            }
            catch (Exception ex)
            {
                feedbackMessage = $"Fehler bei der Erstellung: {ex.Message}";
                feedbackColor = Severity.Error;
            }
        }
        else
        {
            feedbackMessage = "Ungültiger Objekttyp.";
            feedbackColor = Severity.Error;
        }

        _snackbar.Add(feedbackMessage, feedbackColor);
    }
}
