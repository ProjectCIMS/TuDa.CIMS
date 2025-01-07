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
    public string StreetAndNumber { get; set; } = string.Empty;

    /// <summary>
    /// The zip code of the city.
    /// </summary>
    public int? ZipCode { get; set; } = null;

    /// <summary>
    /// The city of the professors address.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// The phone number of the professor.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// The email of the professor.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
