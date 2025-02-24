namespace TuDa.CIMS.Shared.Dtos;

public record UpdateStudentDto : UpdatePersonDto
{
    /// <summary>
    /// The email of the person.
    /// </summary>
    public string? Email { get; set; }
}

