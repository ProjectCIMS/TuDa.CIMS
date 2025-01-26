namespace TuDa.CIMS.Shared.Dtos;

public record UpdateStudentDto
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
    /// The phone number of the person.
    /// </summary>
    public string? PhoneNumber { get; set; }
}
