using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Dtos;

public class AssetItemFilterDto
{
    /// <summary>
    /// The search string for the general search bar
    /// </summary>
    public string? NameOrCas { get; set; }

    /// <summary>
    /// The types of items to be filtered for
    /// </summary>
    public List<AssetItemType>? AssetItemTypes { get; set; }

    /// <summary>
    /// The search string for the Product Column
    /// </summary>
    public string? Product { get; set; }

    /// <summary>
    /// The search string for the Shop Column
    /// </summary>
    public string? Shop { get; set; }

    /// <summary>
    /// The search string for the Item Number Column
    /// </summary>
    public string? ItemNumber { get; set; }

    /// <summary>
    /// The search string for the Room Column
    /// </summary>
    public string? RoomName { get; set; }

    /// <summary>
    /// The search string for the Price Column
    /// </summary>
    public string? Price { get; set; }
}
