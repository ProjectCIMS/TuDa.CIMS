using System.Text.Json.Serialization;

namespace TuDa.CIMS.Shared.Entities;

/// <summary>
/// An entity representing a person.
/// </summary>
[JsonPolymorphic]
[JsonDerivedType(typeof(Professor), nameof(Professor))]
[JsonDerivedType(typeof(Student), nameof(Student))]
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
