namespace TuDa.CIMS.Shared.Dtos.Create;

public class CreateConsumableDto : CreateAssetItemDto
{
    /// <summary>
    /// The amount of the consumable item.
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// The name of the consumable item.
    /// </summary>
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// The serial number of the consumable item.
    /// </summary>
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// If the initial amount of the consumable item should be excluded from statistics.
    /// </summary>
    public bool ExcludeFromConsumableStatistics { get; set; }
}
