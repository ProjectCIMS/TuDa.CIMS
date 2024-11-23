namespace TuDa.CIMS.Shared.Entities;

/// <summary>
/// An entity representing a consumable item in the chemical inventory.
/// </summary>
public record Consumables()
{
    /// <summary>
    /// The name of the consumable item.
    /// </summary>
    public required string Manufacturer { get; set; }
    /// <summary>
    /// The serial number of the consumable item.
    /// </summary>
    public required string SerialNumber { get; set; }
    /// <summary>
    /// The item number of the consumable item.
    /// </summary>
    public required string ItemNumber { get; set; }
    /// <summary>
    /// The shop where the item was purchased.
    /// </summary>
    public required string Shop { get; set; }
};
