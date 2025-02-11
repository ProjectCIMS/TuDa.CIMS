using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Dtos;

public record CreateWorkingGroupDto
{
    /// <summary>
    /// The professor of the group.
    /// </summary>
    public required CreateProfessorDto Professor { get; set; }

    /// <summary>
    /// A phone number to contact the group.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// An email to contact the group.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
