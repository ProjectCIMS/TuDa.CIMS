namespace TuDa.CIMS.Shared.Entities;

/// <summary>
/// An entity representing an item in the chemical inventory.
/// </summary>
public abstract record AssetItem
{
    /// <summary>
    /// An identifier for the item.
    /// </summary>
    public required Guid Id { get; set; }
    /// <summary>
    /// Notes about the item.
    /// </summary>
    public string Note { get; set; } = String.Empty;
    /// <summary>
    /// The room in which the item is located.
    /// </summary>
    public required Room Room { get; set; }
    /// <summary>
    /// The name of the item.
    /// </summary>
    public required string Name { get; set; }
    /// <summary>
    /// The item number of the item.
    /// </summary>
    public required string ItemNumber { get; set; }
    /// <summary>
    /// The shop where the item was purchased.
    /// </summary>
    public required string Shop { get; set; }
}
