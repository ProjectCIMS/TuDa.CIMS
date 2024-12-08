using System.Text.Json.Serialization;

namespace TuDa.CIMS.Shared.Entities;

[JsonPolymorphic]
[JsonDerivedType(typeof(Chemical), nameof(Chemical))]
[JsonDerivedType(typeof(Solvent), nameof(Solvent))]
[JsonDerivedType(typeof(GasCylinder), nameof(GasCylinder))]
public abstract record Substance() : AssetItem
{
    /// <summary>
    /// An unique identifier for the chemical.
    /// </summary>
    public required string Cas { get; set; }

    /// <summary>
    /// The purity of the item.
    /// </summary>
    public required double Purity { get; set; }

    /// <summary>
    /// The price unit of the item.
    /// </summary>
    public required PriceUnits PriceUnit { get; set; }

    /// <summary>
    /// A list of hazards associated with the chemical.
    /// </summary>
    public List<Hazard> Hazards { get; set; } = [];
}
