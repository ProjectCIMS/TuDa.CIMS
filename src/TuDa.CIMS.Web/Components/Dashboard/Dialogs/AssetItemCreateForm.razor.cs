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

    public bool ShowError = false;
    private string FeedbackMessage = string.Empty;
    private Severity FeedbackColor = Severity.Success;

    /// <summary>
    /// To choose from the different forms for inputting
    /// </summary>
    private AssetItemType _selectedAssetItemType = AssetItemType.Chemical;

    private AssetItemForm _assetItemForm;
    private ChemicalItemForm _chemicalItemForm;
    private ConsumableItemForm _consumableItemForm;
    private GasCylinderItemForm _gasCylinderForm;

    /// <summary>
    /// Temporary Values to bind when inputting
    /// </summary>
    private string Name { get; set; } = string.Empty;
    private string Shop { get; set; } = string.Empty;
    private string ItemNumber { get; set; } = string.Empty;
    private string Note { get; set; } = string.Empty;
    private string Cas { get; set; } = string.Empty;
    private string Manufacturer { get; set; } = string.Empty;
    private string SerialNumber { get; set; } = string.Empty;
    private double Price { get; set; } = 0.0;
    private int ConsumableAmount { get; set; } = 0;
    private string Purity { get; set; } = string.Empty;
    private double BindingSize { get; set; } = 0.0;
    private double Volume { get; set; } = 0.0;
    private double Pressure { get; set; } = 0.0;
    private Room Room { get; set; } = new Room() { Name = string.Empty };
    private MeasurementUnits PriceUnit { get; set; } = MeasurementUnits.Piece;

    private void ResetInputs()
    {
        Name = string.Empty;
        Shop = string.Empty;
        ItemNumber = string.Empty;
        Note = string.Empty;
        Cas = string.Empty;
        Manufacturer = string.Empty;
        SerialNumber = string.Empty;
        Price = 0.0;
        ConsumableAmount = 0;
        Purity = string.Empty;
        BindingSize = 0.0;
        Volume = 0.0;
        Pressure = 0.0;
        Room.Name = string.Empty;
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
                Name = Name,
                Shop = Shop,
                ItemNumber = ItemNumber,
                Note = Note,
                Cas = Cas,
                Price = Price,
                Purity = Purity,
                PriceUnit = PriceUnit,
                Room = Room,
            },

            AssetItemType.Consumable => new Consumable
            {
                Name = Name,
                Shop = Shop,
                ItemNumber = ItemNumber,
                Note = Note,
                Manufacturer = Manufacturer,
                SerialNumber = SerialNumber,
                Price = Price,
                Amount = ConsumableAmount,
                Room = Room,
            },

            AssetItemType.GasCylinder => new GasCylinder
            {
                Name = Name,
                Shop = Shop,
                ItemNumber = ItemNumber,
                Note = Note,
                Cas = Cas,
                Purity = Purity,
                Price = Price,
                Volume = Volume,
                Pressure = Pressure,
                Room = Room,
                PriceUnit = PriceUnit,
            },

            AssetItemType.Solvent => new Solvent
            {
                Name = Name,
                Shop = Shop,
                ItemNumber = ItemNumber,
                Note = Note,
                Cas = Cas,
                Price = Price,
                Purity = Purity,
                Room = Room,
                PriceUnit = PriceUnit,
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
