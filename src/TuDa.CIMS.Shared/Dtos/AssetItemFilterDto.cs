using Refit;
using TuDa.CIMS.Shared.Entities.Constants;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Dtos;

public class AssetItemFilterDto
{
    /// <summary>
    /// The search string for the general search bar
    /// </summary>
    [Query]
    public string? NameOrCas { get; set; }

    /// <summary>
    /// The types of items to be filtered for
    /// </summary>
    [Query(CollectionFormat = CollectionFormat.Multi)]
    public List<AssetItemType> AssetItemTypes { get; set; } = [];

    /// <summary>
    /// The Columns to be filtered for
    /// </summary>
    [Query(CollectionFormat = CollectionFormat.Multi)]
    public List<string> AssetItemColumnsList { get; set; } = [];

    /// <summary>
    /// The search string for the Product Column
    /// </summary>
    [Query]
    public string? Product { get; set; } = string.Empty;

    /// <summary>
    /// The search string for the Shop Column
    /// </summary>
    [Query]
    public string? Shop { get; set; } = string.Empty;

    /// <summary>
    /// The search string for the Item Number Column
    /// </summary>
    [Query]
    public string? ItemNumber { get; set; } = string.Empty;

    /// <summary>
    /// The search string for the Room Column
    /// </summary>
    [Query]
    public string? RoomName { get; set; } = string.Empty;

    /// <summary>
    /// The search string for the Price Column
    /// </summary>
    [Query]
    public string? Price { get; set; } = string.Empty;
}
