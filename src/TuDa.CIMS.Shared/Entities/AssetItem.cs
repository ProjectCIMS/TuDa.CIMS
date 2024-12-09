using System.Text.Json.Serialization;

namespace TuDa.CIMS.Shared.Entities;

/// <summary>
/// An entity representing an item in the chemical inventory.
/// </summary>
[JsonPolymorphic]
[JsonDerivedType(typeof(Consumable), nameof(Consumable))]
[JsonDerivedType(typeof(Chemical), nameof(Chemical))]
[JsonDerivedType(typeof(Solvent), nameof(Solvent))]
[JsonDerivedType(typeof(GasCylinder), nameof(GasCylinder))]
public abstract record AssetItem : BaseEntity
{
    /// <summary>
    /// The name of the item.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The price of the item.
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// The room in which the item is located.
    /// </summary>
    public required Room Room { get; set; }

    /// <summary>
    /// The item number of the item.
    /// </summary>
    public string ItemNumber { get; set; } = string.Empty;

    /// <summary>
    /// The shop where the item was purchased.
    /// </summary>
    public string Shop { get; set; } = string.Empty;

    /// <summary>
    /// Notes about the item.
    /// </summary>
    public string Note { get; set; } = string.Empty;
}
