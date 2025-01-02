using System.Text.Json.Serialization;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Entities;

/// <summary>
/// An entity representing a substance.
/// </summary>
[JsonPolymorphic]
[JsonDerivedType(typeof(Chemical), nameof(Chemical))]
[JsonDerivedType(typeof(Solvent), nameof(Solvent))]
[JsonDerivedType(typeof(GasCylinder), nameof(GasCylinder))]
public abstract record Substance : AssetItem
{
    /// <summary>
    /// A unique identifier for the substance.
    /// </summary>
    public required string Cas { get; set; }

    /// <summary>
    /// The purity of the substance.
    /// </summary>
    public string Purity { get; set; } = string.Empty;

    /// <summary>
    /// The unit of measurement for the price of the substance.
    /// </summary>
    public required MeasurementUnits PriceUnit { get; set; }

    /// <summary>
    /// A list of hazards associated with the substance.
    /// </summary>
    public List<Hazard> Hazards { get; set; } = [];
}
