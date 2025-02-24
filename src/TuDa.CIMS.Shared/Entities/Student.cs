namespace TuDa.CIMS.Shared.Entities;

public record Student : Person
{
    /// <summary>
    /// The email of the person.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
