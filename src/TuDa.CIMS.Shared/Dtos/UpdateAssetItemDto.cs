namespace TuDa.CIMS.Shared.Dtos;

public abstract class UpdateAssetItemDto
{
    /// <summary>
    /// Notes about the item.
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// The room in which the item is located.
    /// </summary>
    public Guid? RoomId { get; set; }

    /// <summary>
    /// The name of the item.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The item number of the item.
    /// </summary>
    public string? ItemNumber { get; set; }

    /// <summary>
    /// The shop where the item was purchased.
    /// </summary>
    public string? Shop { get; set; }
}
