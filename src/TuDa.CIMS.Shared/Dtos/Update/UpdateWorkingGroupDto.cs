namespace TuDa.CIMS.Shared.Dtos.Update;

public record UpdateWorkingGroupDto
{
    /// <summary>
    /// The professor of the group.
    /// </summary>
    public UpdateProfessorDto? Professor { get; set; }

    /// <summary>
    /// A phone number to contact the group.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// An email to contact the group.
    /// </summary>
    public string? Email { get; set; }
}
