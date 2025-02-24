namespace TuDa.CIMS.Shared.Dtos;

public record CreateStudentDto : CreatePersonDto
{
    /// <summary>
    /// The email of the student.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
