using System.Text.Json.Serialization;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Dtos;

[JsonPolymorphic]
[JsonDerivedType(typeof(UpdateProfessorDto), nameof(UpdateProfessorDto))]
[JsonDerivedType(typeof(UpdateStudentDto), nameof(UpdateStudentDto))]
public record UpdatePersonDto
{
    /// <summary>
    /// The name of the person.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The first name of the person.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// The gender of the person.
    /// </summary>
    public Gender? Gender { get; set; }

    /// <summary>
    /// The phone number of the person.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;
}
