namespace TuDa.CIMS.Shared.Dtos;

public class CreateStudentDto
{
    /// <summary>
    /// The name of the person.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The first name of the person.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
}
