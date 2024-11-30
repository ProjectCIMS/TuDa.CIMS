namespace TuDa.CIMS.Shared.Dtos;

public class UpdateConsumableItemDto : UpdateAssetItemDto
{
    /// <summary>
    /// The name of the consumable item.
    /// </summary>
    public string? Manufacturer { get; set; }

    /// <summary>
    /// The serial number of the consumable item.
    /// </summary>
    public string? SerialNumber { get; set; }
}
