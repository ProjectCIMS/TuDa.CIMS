namespace TuDa.CIMS.Shared.Entities;

public record Professor : Person
{
    /// <summary>
    /// The title of the professor.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The address of the professor.
    /// </summary>
    public Address Address { get; set; } = new Address();
}
