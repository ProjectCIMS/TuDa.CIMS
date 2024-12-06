namespace TuDa.CIMS.Shared.Entities;

/// <summary>
/// An entity representing a hazard.
/// </summary>
public record Hazard : BaseEntity
{
    /// <summary>
    /// The name of the hazard.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// An Image of the hazard.
    /// </summary>
    public string ImagePath { get; set; } = string.Empty;

    /// <summary>
    /// All substances that have this Hazard.
    /// </summary>
    public List<Substance> Substances { get; set; } = [];
}
