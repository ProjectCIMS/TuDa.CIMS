namespace TuDa.CIMS.Shared.Entities;

/// <summary>
/// An entity representing a consumable item in the chemical inventory.
/// </summary>
public record Consumable : AssetItem
{
    /// <summary>
    /// The name of the consumable item.
    /// </summary>
    public required string Manufacturer { get; set; }

    /// <summary>
    /// The serial number of the consumable item.
    /// </summary>
    public required string SerialNumber { get; set; }
}
