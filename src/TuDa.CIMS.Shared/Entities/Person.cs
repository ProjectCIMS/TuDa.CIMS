using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Entities;

/// <summary>
/// An entity representing a person.
/// </summary>
public abstract record Person : BaseEntity
{
    /// <summary>
    /// The name of the person.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The first name of the person.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// The working group the person is member of.
    /// </summary>
    public required WorkingGroup WorkingGroup { get; set; }
}
