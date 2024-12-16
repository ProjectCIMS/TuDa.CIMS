namespace TuDa.CIMS.Shared.Dtos;

public class CreateConsumableDto: CreateAssetItemDto
{
    /// <summary>
    /// The amount of the consumable item.
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// The name of the consumable item.
    /// </summary>
    public string Manufacturer { get; set; }

    /// <summary>
    /// The serial number of the consumable item.
    /// </summary>
    public string SerialNumber { get; set; }
}
