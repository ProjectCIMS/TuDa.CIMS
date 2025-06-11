using System.Text.Json.Serialization;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Dtos.Update;

[JsonPolymorphic]
[JsonDerivedType(typeof(UpdateChemicalDto), nameof(UpdateChemicalDto))]
[JsonDerivedType(typeof(UpdateSolventDto), nameof(UpdateSolventDto))]
[JsonDerivedType(typeof(UpdateGasCylinderDto), nameof(UpdateGasCylinderDto))]
public abstract record UpdateSubstanceDto : UpdateAssetItemDto
{
    /// <summary>
    /// A unique identifier for the chemical.
    /// </summary>
    public string? Cas { get; set; }

    /// <summary>
    /// The purity of the item.
    /// </summary>
    public string? Purity { get; set; }

    /// <summary>
    /// The price unit of the item.
    /// </summary>
    public MeasurementUnits? PriceUnit { get; set; }

    /// <summary>
    /// A list of hazards associated with the chemical.
    /// </summary>
    public List<Hazard>? Hazards { get; set; }
}
