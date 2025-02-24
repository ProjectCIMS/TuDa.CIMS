using System.Text.Json.Serialization;
using TuDa.CIMS.Shared.Entities.Enums;

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
    /// The gender of the person.
    /// </summary>
    public Gender Gender { get; set; } = Gender.Unknown;

    /// <summary>
    /// The phone number of the person.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

}
