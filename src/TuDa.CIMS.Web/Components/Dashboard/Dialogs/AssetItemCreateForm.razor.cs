using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Web.Components.Dashboard.Dialogs;

public partial class AssetItemCreateForm
{
    /// <summary>
    /// To choose from the different forms for inputting
    /// </summary>
    private AssetItemType _selectedAssetItemType = AssetItemType.Chemical;

    /// <summary>
    /// Temporary Values to bind when inputting
    /// TODO: maybe add set function so that price and other numerics can't be negative
    /// </summary>
    private string Name { get; set; }
    private string Shop { get; set; }
    private string ItemNumber { get; set; }
    private string Note { get; set; }
    private string Cas { get; set; }
    private string Manufacturer { get; set; }
    private string SerialNumber { get; set; }
    private double Price { get; set; }
    private int ConsumableAmount { get; set; }
    private string Purity { get; set; }
    private double BindingSize { get; set; }
    private double Volume { get; set; }
    private double Pressure { get; set; }
    private Room Room { get; set; }
    private MeasurementUnits PriceUnit { get; set; }

    /// <summary>
    /// Item after clicking "Änderungen speichern" Button
    /// </summary>
    protected AssetItem savedItem;

    [Parameter]
    public EventCallback<AssetItem> OnSaveComplete { get; set; }

    protected async Task HandleSave()
    {
        savedItem = SaveChanges();

        if (OnSaveComplete.HasDelegate)
        {
            await OnSaveComplete.InvokeAsync(savedItem);
        }
    }

    /// <summary>
    /// Functionality of the "Änderungen speichern" Button: Bind the Values to the actual Item
    /// </summary>
    protected AssetItem SaveChanges()
    {
        switch (_selectedAssetItemType)
        {
            case AssetItemType.Chemical:
                return savedItem = new Chemical
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
                };

            case AssetItemType.Consumable:
                return savedItem = new Consumable
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
                };

            case AssetItemType.GasCylinder:
                return savedItem = new GasCylinder
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
                };

            case AssetItemType.Solvent:
                return savedItem = new Solvent
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
                };

            default:
                return savedItem;
        }
    }
}
