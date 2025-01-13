using Microsoft.AspNetCore.Components;
using MudBlazor;
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
        AssetItem? savedItem = _selectedAssetItemType switch
        {
            AssetItemType.Chemical => new Chemical
            {
                Name = _assetItemForm.FormName,
                Shop = _assetItemForm.FormShop,
                ItemNumber = _assetItemForm.FormItemNumber,
                Note = _assetItemForm.FormNote,
                Cas = _chemicalItemForm.FormCas,
                Price = _assetItemForm.FormPrice,
                Purity = _chemicalItemForm.FormPurity,
                PriceUnit = _chemicalItemForm.FormPriceUnit,
                Room = new Room() { Name = _assetItemForm.FormRoomName }
            },

            AssetItemType.Consumable => new Consumable
            {
                Name = _assetItemForm.FormName,
                Shop = _assetItemForm.FormShop,
                ItemNumber = _assetItemForm.FormItemNumber,
                Note = _assetItemForm.FormNote,
                Price = _assetItemForm.FormPrice,
                Room = new Room() { Name = _assetItemForm.FormRoomName },
                Manufacturer = _consumableItemForm.FormManufacturer,
                SerialNumber = _consumableItemForm.FormSerialNumber,
                Amount = _consumableItemForm.FormConsumableAmount
            },

            AssetItemType.GasCylinder => new GasCylinder
            {
                Name = _assetItemForm.FormName,
                Shop = _assetItemForm.FormShop,
                ItemNumber = _assetItemForm.FormItemNumber,
                Note = _assetItemForm.FormNote,
                Price = _assetItemForm.FormPrice,
                Room = new Room() { Name = _assetItemForm.FormRoomName },
                Cas = _chemicalItemForm.FormCas,
                Purity = _chemicalItemForm.FormPurity,
                Volume = _gasCylinderForm.FormVolume,
                Pressure = _gasCylinderForm.FormPressure,
                PriceUnit = _chemicalItemForm.FormPriceUnit
            },

            AssetItemType.Solvent => new Solvent
            {
                Name = _assetItemForm.FormName,
                Shop = _assetItemForm.FormShop,
                ItemNumber = _assetItemForm.FormItemNumber,
                Note = _assetItemForm.FormNote,
                Cas = _chemicalItemForm.FormCas,
                Price = _assetItemForm.FormPrice,
                Purity = _chemicalItemForm.FormPurity,
                PriceUnit = _chemicalItemForm.FormPriceUnit,
                Room = new Room() { Name = _assetItemForm.FormRoomName }
            },

            _ => null,
        };

        if (savedItem != null)
        {
            try
            {
                //TODO: #41 needed
                //await _assetItemApi.CreateAsync(savedItem);
                FeedbackMessage = "Das Objekt wurde erfolgreich erstellt.";
                FeedbackColor = Severity.Success;
            }
            catch (Exception ex)
            {
                FeedbackMessage = $"Fehler bei der Erstellung: {ex.Message}";
                FeedbackColor = Severity.Error;
            }
        }
        else
        {
            FeedbackMessage = "Ungültiger Objekttyp.";
            FeedbackColor = Severity.Error;
        }

        _snackbar.Add(FeedbackMessage, FeedbackColor);
    }
}
