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

    private SelectionMode _selectionMode = SelectionMode.SingleSelection;
    private Color _selectedColor;

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
    /// Items to be created
    /// </summary>
    private Chemical _createdChemicalItem = new Chemical()
    {
        Name = "",
        Shop = "",
        ItemNumber = "",
        Note = "",
        Id = Guid.NewGuid(),
        Room = new Room() { Name = "", Id = Guid.NewGuid() },
        Price = 0,
        Hazards = new List<Hazard>(),
        Cas = "",
        Purity = "",
        PriceUnit = MeasurementUnits.Piece,
        BindingSize = 0,
    };

    private Consumable _createdConsumableItem = new Consumable()
    {
        Name = "",
        Shop = "",
        ItemNumber = "",
        Note = "",
        Id = Guid.NewGuid(),
        Room = new Room() { Name = "", Id = Guid.NewGuid() },
        Price = 0,
        Manufacturer = "",
        SerialNumber = "",
        Amount = 0,
    };

    private GasCylinder _createdGasCylinder = new GasCylinder()
    {
        Name = "",
        Shop = "",
        ItemNumber = "",
        Note = "",
        Id = Guid.NewGuid(),
        Room = new Room() { Name = "", Id = Guid.NewGuid() },
        Price = 0,
        Hazards = new List<Hazard>(),
        Cas = "",
        Purity = "",
        PriceUnit = MeasurementUnits.Piece,
        Volume = 0,
        Pressure = 0,
    };

    private Solvent _createdSolventItem = new Solvent()
    {
        Name = "",
        Shop = "",
        ItemNumber = "",
        Note = "",
        Id = Guid.NewGuid(),
        Room = new Room() { Name = "", Id = Guid.NewGuid() },
        Price = 0,
        Hazards = new List<Hazard>(),
        Cas = "",
        Purity = "",
        PriceUnit = MeasurementUnits.Piece,
        BindingSize = 0,
    };

    /// <summary>
    /// Functionality of the Save Button: Bind the Values to the actual Item
    /// </summary>
    protected void SaveChanges()
    {
        switch (_selectedItemType)
        {
            case ItemType.Chemical:
                _createdChemicalItem.Name = Name;
                _createdChemicalItem.Shop = Shop;
                _createdChemicalItem.ItemNumber = ItemNumber;
                _createdChemicalItem.Note = Note;
                _createdChemicalItem.Cas = Cas;
                _createdChemicalItem.Price = Price;
                _createdChemicalItem.Purity = Purity;
                break;
                ;

            case ItemType.Consumable:
                _createdConsumableItem.Name = Name;
                _createdConsumableItem.Shop = Shop;
                _createdConsumableItem.ItemNumber = ItemNumber;
                _createdConsumableItem.Note = Note;
                _createdConsumableItem.Manufacturer = Manufacturer;
                _createdConsumableItem.SerialNumber = SerialNumber;
                _createdConsumableItem.Price = Price;
                _createdConsumableItem.Amount = ConsumableAmount;
                break;
                ;

            case ItemType.GasCylinder:
                _createdGasCylinder.Name = Name;
                _createdGasCylinder.Shop = Shop;
                _createdGasCylinder.ItemNumber = ItemNumber;
                _createdGasCylinder.Note = Note;
                _createdGasCylinder.Cas = Cas;
                _createdGasCylinder.Purity = Purity;
                _createdGasCylinder.Price = Price;
                _createdGasCylinder.Volume = Volume;
                _createdGasCylinder.Pressure = Pressure;
                break;

            case ItemType.Solvent:
                _createdSolventItem.Name = Name;
                _createdSolventItem.Shop = Shop;
                _createdSolventItem.ItemNumber = ItemNumber;
                _createdSolventItem.Note = Note;
                _createdSolventItem.Cas = Cas;
                _createdSolventItem.Price = Price;
                _createdSolventItem.Purity = Purity;
                break;
                ;
        }
    }
}
