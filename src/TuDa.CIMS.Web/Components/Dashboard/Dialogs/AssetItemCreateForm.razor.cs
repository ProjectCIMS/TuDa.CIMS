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
    private ItemType _selectedItemType = ItemType.Chemical;

    /// <summary>
    /// Available types of Items
    /// </summary>
    public enum ItemType
    {
        Chemical,
        Consumable,
        GasCylinder,
        Solvent,
    }

    private readonly SelectionMode _selectionMode = SelectionMode.SingleSelection;

    /// <summary>
    /// Temporary Values to bind when inputting
    /// </summary>
    protected string Name = "";
    protected string Shop = "";
    protected string ItemNumber = "";
    protected string Note = "";
    protected string Cas = "";
    protected string Manufacturer = "";
    protected string SerialNumber = "";
    protected double Price = 0;
    protected int ConsumableAmount = 0;
    protected string Purity = "";
    protected double BindingSize = 0;
    protected double Volume = 0;
    protected double Pressure = 0;
    protected Room Room = new Room() { Name = string.Empty };
    protected MeasurementUnits PriceUnit = MeasurementUnits.Piece;

    /// <summary>
    /// Method to switch to a Substance
    /// </summary>
    private void SwitchToChemical()
    {
        _selectedItemType = ItemType.Chemical;
    }

    /// <summary>
    /// Method to switch to a Consumable
    /// </summary>
    private void SwitchToConsumable()
    {
        _selectedItemType = ItemType.Consumable;
    }

    private void SwitchToGasCylinder()
    {
        _selectedItemType = ItemType.GasCylinder;
    }

    private void SwitchToSolvent()
    {
        _selectedItemType = ItemType.Solvent;
    }

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
        switch (_selectedItemType)
        {
            case ItemType.Chemical:
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

            case ItemType.Consumable:
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

            case ItemType.GasCylinder:
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

            case ItemType.Solvent:
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
