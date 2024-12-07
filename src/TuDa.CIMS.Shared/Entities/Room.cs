namespace TuDa.CIMS.Shared.Entities;

/// <summary>
/// An entity representing a room in the chemical department.
/// </summary>
public record Room : BaseEntity
{
    /// <summary>
    /// The name of the room.
    /// </summary>
    public required string Name { get; set; }
}
