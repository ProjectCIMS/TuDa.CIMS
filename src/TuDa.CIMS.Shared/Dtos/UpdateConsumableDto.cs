namespace TuDa.CIMS.Shared.Dtos;

public record UpdateConsumableDto : UpdateAssetItemDto
{
    /// <summary>
    /// The amount of the consumable item.
    /// </summary>
    public required double? Amount { get; set; }

    /// <summary>
    /// The name of the consumable item.
    /// </summary>
    public required string? Manufacturer { get; set; }

    /// <summary>
    /// The serial number of the consumable item.
    /// </summary>
    public required string? SerialNumber { get; set; }
}
