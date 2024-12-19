namespace TuDa.CIMS.Shared.Entities;

public record Professor : Person
{
    /// <summary>
    /// Address of the professor.
    /// </summary>
    public required Address Address { get; set; }

    /// <summary>
    /// The title of the Professor as abbreviation.
    /// </summary>
    public string Title { get; set; } = "Prof.";
}
