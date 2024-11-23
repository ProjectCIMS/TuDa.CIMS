namespace TuDa.CIMS.Shared.Entities;

/// <summary>
/// An entity representing a chemical.
/// </summary>
public record Chemical : AssetItem
{
    /// <summary>
    /// An unique identifier for the chemical.
    /// </summary>
    public required string Cas { get; set; }
    /// <summary>
    /// A list of hazards associated with the chemical.
    /// </summary>
    public List<Hazard> Hazards { get; set; } = [];
    /// <summary>
    /// The unit of the chemical.
    /// </summary>
    public required string Unit { get; set; }
}
