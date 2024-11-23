namespace TuDa.CIMS.Shared.Entities;

/// <summary>
/// An entity representing a room in the chemical department.
/// </summary>
public record Room()
{
/// <summary>
/// An identifier for the room.
/// </summary>
    public required Guid Id { get; set; }
/// <summary>
/// The name of the room.
/// </summary>
    public required string Name { get; set; }
};
