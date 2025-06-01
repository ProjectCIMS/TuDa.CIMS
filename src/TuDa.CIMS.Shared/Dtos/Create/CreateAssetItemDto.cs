using System.Text.Json.Serialization;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Dtos.Create;

[JsonPolymorphic]
[JsonDerivedType(typeof(CreateSolventDto), nameof(CreateSolventDto))]
[JsonDerivedType(typeof(CreateChemicalDto), nameof(CreateChemicalDto))]
[JsonDerivedType(typeof(CreateGasCylinderDto), nameof(CreateGasCylinderDto))]
[JsonDerivedType(typeof(CreateConsumableDto), nameof(CreateConsumableDto))]
public class CreateAssetItemDto
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
    public required Rooms Room { get; set; }

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
