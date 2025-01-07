using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Web.Components.Dashboard;

public partial class AssetList
{
    /// <summary>
    /// List of items to be shown
    /// </summary>
    [Parameter]
    public List<AssetItem> AssetItems { get; set; } = s_sampleValues;

    private static readonly List<AssetItem> s_sampleValues =
    [
        new GasCylinder
        {
            Id = Guid.Empty,
            Name = "Trockeneis",
            Price = 33,
            Room = new Room { Id = Guid.Empty, Name = "Audimax" },
            ItemNumber = "1845",
            Shop = "Eisladen",
            Note = "Temperatur: -78,5\u00b0 C",
            Cas = "124-38-9",
            Purity = "0.8",
            PriceUnit = MeasurementUnits.KiloGram,
            Hazards =
            [
                new Hazard
                {
                    Id = Guid.Empty,
                    Name = "kalt",
                    ImagePath = "",
                },
            ],
            Volume = 2,
            Pressure = 4,
        },
        new Chemical
        {
            Id = Guid.Empty,
            Name = "Ethanol",
            Price = 4.50,
            Room = new Room { Id = Guid.Empty, Name = "Bistro Athene" },
            ItemNumber = "1170",
            Shop = "Taverne",
            Note = "AKA Alkohol",
            Cas = "64-17-5",
            Purity = "1.33",
            PriceUnit = MeasurementUnits.Liter,
            Hazards =
            [
                new Hazard
                {
                    Id = Guid.Empty,
                    Name = "macht süchtig",
                    ImagePath = "",
                },
            ],
            BindingSize = 7,
        },
        new Solvent
        {
            Id = Guid.Empty,
            Name = "Natriumhydroxid",
            Price = 6.5,
            Room = new Room { Id = Guid.Empty, Name = "Bosch-Hörsaal" },
            ItemNumber = "1823",
            Shop = "ALDI",
            Note = "Feststoff",
            Cas = "1310-73-2",
            Purity = "0.5",
            PriceUnit = MeasurementUnits.Piece,
            Hazards =
            [
                new Hazard
                {
                    Id = Guid.Empty,
                    Name = "ätzend",
                    ImagePath = "",
                },
            ],
            BindingSize = 3,
        },
        new Consumable
        {
            Id = Guid.Empty,
            Name = "Reagenzglas",
            Price = 5,
            Room = new Room { Id = Guid.Empty, Name = "Glashaus/10" },
            ItemNumber = "2770/30",
            Shop = "Mediamarkt",
            Note = "zerbrechlich",
            Amount = 2,
            Manufacturer = "Glasmacher",
            SerialNumber = "12345678910",
        },
    ];

    /// <summary>
    /// Returns type of the given item.
    /// </summary>
    private static string GetItemType(AssetItem assetItem) =>
        assetItem switch
        {
            Solvent => "Lösungsmittel",
            Chemical => "Chemikalie",
            Consumable => "Verbrauchsmaterial",
            GasCylinder => "Druckgasflasche",
            _ => "-",
        };

    /// <summary>
    /// Returns the CAS number.
    /// </summary>
    private static string GetCas(AssetItem assetItem) =>
        assetItem switch
        {
            Substance substance => substance.Cas,
            _ => "-",
        };

    /// <summary>
    /// Returns the purity of the item.
    /// </summary>
    private static string GetPurity(AssetItem assetItem) =>
        assetItem switch
        {
            Substance substance => substance.Purity,
            _ => "-",
        };

    /// <summary>
    /// Returns the list of hazards of the item.
    /// </summary>
    private static List<Hazard> GetHazards(AssetItem assetItem) =>
        assetItem switch
        {
            Chemical chemical => chemical.Hazards,
            _ => [],
        };

    /// <summary>
    /// Returns the price unit of the substance.
    /// </summary>
    private static string GetPriceUnit(AssetItem assetItem) =>
        assetItem switch
        {
            Substance substance => substance.PriceUnit.ToString(),
            _ => "-",
        };

    /// <summary>
    /// Returns the binding size of the chemical or solvent.
    /// </summary>
    private static double GetBindingSize(AssetItem assetItem) =>
        assetItem switch
        {
            Chemical chemical => chemical.BindingSize,
            _ => 0,
        };

    /// <summary>
    /// Returns the volume of the pressured gas cylinder.
    /// </summary>
    private static double GetVolume(AssetItem assetItem) =>
        assetItem switch
        {
            GasCylinder gasCylinder => gasCylinder.Volume,
            _ => 0,
        };

    /// <summary>
    /// Returns the pressure of the pressured gas cylinder.
    /// </summary>
    private static double GetPressure(AssetItem assetItem) =>
        assetItem switch
        {
            GasCylinder gasCylinder => gasCylinder.Pressure,
            _ => 0,
        };

    /// <summary>
    /// Returns the manufacturer of the consumable.
    /// </summary>
    private static double GetAmount(AssetItem assetItem) =>
        assetItem switch
        {
            Consumable consumable => consumable.Amount,
            _ => 0,
        };

    /// <summary>
    /// Returns the manufacturer of the consumable.
    /// </summary>
    private static string GetManufacturer(AssetItem assetItem) =>
        assetItem switch
        {
            Consumable consumable => consumable.Manufacturer,
            _ => "-",
        };

    /// <summary>
    /// Returns the serial number of the consumable.
    /// </summary>
    private static string GetSerialNumber(AssetItem assetItem) =>
        assetItem switch
        {
            Consumable consumable => consumable.SerialNumber,
            _ => "-",
        };
}
