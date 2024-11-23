namespace TuDa.CIMS.Shared.Entities;

/// <summary>
/// An entity representing a hazard.
/// </summary>
public record Hazard()
{
    /// <summary>
    /// An identifier for the hazard.
    /// </summary>
    public required Guid Id { get; set; }
    /// <summary>
    /// The name of the hazard.
    /// </summary>
    public required string Name { get; set; }
    /// <summary>
    /// An Image of the hazard.
    /// </summary>
    public string ImagePath { get; set; } = string.Empty;
};
