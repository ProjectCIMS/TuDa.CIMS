using System.Text.Json.Serialization;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Dtos;

[JsonPolymorphic]
[JsonDerivedType(typeof(UpdateSolventDto), nameof(UpdateSolventDto))]
[JsonDerivedType(typeof(UpdateChemicalDto), nameof(UpdateChemicalDto))]
[JsonDerivedType(typeof(UpdateGasCylinderDto), nameof(UpdateGasCylinderDto))]
[JsonDerivedType(typeof(UpdateConsumableDto), nameof(UpdateConsumableDto))]
public abstract record UpdateAssetItemDto
{
    /// <summary>
    /// Notes about the item.
    /// </summary>
    public string? Note { get; set; } = String.Empty;

    /// <summary>
    /// The room in which the item is located.
    /// </summary>
    public required Guid? RoomId { get; set; }

    /// <summary>
    /// The name of the item.
    /// </summary>
    public required string? Name { get; set; }

    /// <summary>
    /// The item number of the item.
    /// </summary>
    public required string? ItemNumber { get; set; }

    /// <summary>
    /// The shop where the item was purchased.
    /// </summary>
    public required string? Shop { get; set; }

    /// <summary>
    /// The price of the item.
    /// </summary>
    public required double? Price { get; set; }
}
