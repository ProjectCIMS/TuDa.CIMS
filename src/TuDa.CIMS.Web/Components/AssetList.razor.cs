using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;


namespace TuDa.CIMS.Web.Components;

public partial class AssetList
{
    /// <summary>
    /// List of items to be shown
    /// </summary>
    [Parameter]
    public List<AssetItem> AssetItems { get; set; } = s_sampleValues;

    private static readonly List<AssetItem> s_sampleValues =
    [
        new Chemical
        {
            Id = Guid.Empty,
            Note = "Temperatur: -78,5\u00b0 C",
            Room = new Room { Id = Guid.Empty, Name = "Audimax" },
            Name = "Trockeneis",
            Cas = "124-38-9",
            ItemNumber = "1845",
            Shop = "Eisladen",
            Hazards = [new Hazard { Id = Guid.Empty, Name = "kalt", ImagePath = "" }],
            Unit = "kg"
        },
        new Chemical
        {
            Id = Guid.Empty,
            Note = "AKA Alkohol",
            Room = new Room { Id = Guid.Empty, Name = "Bistro Athene" },
            Name = "Ethanol",
            Cas = "64-17-5",
            ItemNumber = "1170",
            Shop = "Taverne",
            Hazards = [new Hazard { Id = Guid.Empty, Name = "macht süchtig", ImagePath = "" }],
            Unit = "l"
        },
        new Chemical
        {
            Id = Guid.Empty,
            Note = "Feststoff",
            Room = new Room { Id = Guid.Empty, Name = "Bosch-Hörsaal" },
            Name = "Natriumhydroxid",
            Cas = "1310-73-2",
            ItemNumber = "1823",
            Shop = "ALDI",
            Hazards = [new Hazard { Id = Guid.Empty, Name = "ätzend", ImagePath = "" }],
            Unit = "kg"
        },
        new Consumable
        {
            Id = Guid.Empty,
            Note = "zerbrechlich",
            Room = new Room { Id = Guid.Empty, Name = "Glashaus/10" },
            Name = "Reagenzglas",
            ItemNumber = "2770/30",
            Shop = "Mediamarkt",
            Manufacturer = "Glasmacher",
            SerialNumber = "12345678910"
        }
    ];

    /// <summary>
    /// Returns type of the given item.
    /// </summary>
    private static string GetItemType(AssetItem assetItem) =>
    assetItem switch
    {
        Chemical => "Chemikalie",
        Consumable => "Verbrauchsmaterial",
        _ => "-",
    };

    /// <summary>
    /// Returns the CAS number.
    /// </summary>
    private static string GetCas(AssetItem assetItem)
        => assetItem switch
        {
            Chemical chemical => chemical.Cas,
            _ => "-"
        };

    /// <summary>
    /// Returns the list of hazards of the item.
    /// </summary>
    private static List<Hazard> GetHazards(AssetItem assetItem)
        => assetItem switch
        {
            Chemical chemical => chemical.Hazards,
            _ => []
        };

    /// <summary>
    /// Returns the unit of the chemical.
    /// </summary>
    private static string GetUnit(AssetItem assetItem)
        => assetItem switch
        {
            Chemical chemical => chemical.Unit,
            _ => "-"
        };

    /// <summary>
    /// Returns the manufacturer of the consumable.
    /// </summary>
    private static string GetManufacturer(AssetItem assetItem)
        => assetItem switch
        {
            Consumable consumable => consumable.Manufacturer,
            _ => "-"
        };

    /// <summary>
    /// Returns the serial number of the consumable.
    /// </summary>
    private static string GetSerialNumber(AssetItem assetItem)
        => assetItem switch
        {
            Consumable consumable => consumable.SerialNumber,
            _ => "-"
        };
}
