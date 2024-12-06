using System.Text.Json.Serialization;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Dtos;

[JsonPolymorphic]
[JsonDerivedType(typeof(UpdateChemicalDto), nameof(UpdateChemicalDto))]
[JsonDerivedType(typeof(UpdateSolventDto), nameof(UpdateSolventDto))]
[JsonDerivedType(typeof(UpdateGasCylinderDto), nameof(UpdateGasCylinderDto))]
public abstract record UpdateSubstanceDto : UpdateAssetItemDto
{
    /// <summary>
    /// An unique identifier for the chemical.
    /// </summary>
    public required string? Cas { get; set; }

    /// <summary>
    /// The purity of the item.
    /// </summary>
    public required double? Purity { get; set; }

    /// <summary>
    /// The price unit of the item.
    /// </summary>
    public required PriceUnits? PriceUnit { get; set; }

    /// <summary>
    /// A list of hazards associated with the chemical.
    /// </summary>
    public List<Hazard>? Hazards { get; set; } = [];
}
