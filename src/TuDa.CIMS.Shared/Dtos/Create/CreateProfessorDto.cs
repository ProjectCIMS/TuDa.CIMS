namespace TuDa.CIMS.Shared.Dtos.Create;

public record CreateProfessorDto : CreatePersonDto
{
    /// <summary>
    /// The title of the professor.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The street of the professor.
    /// </summary>
    public string AddressStreet { get; set; } = string.Empty;

    /// <summary>
    /// The number of the street.
    /// </summary>
    public int AddressNumber { get; set; } = 0;

    /// <summary>
    /// The zip code of the city.
    /// </summary>
    public string AddressZipCode { get; set; } = string.Empty;

    /// <summary>
    /// The city of the professors address.
    /// </summary>
    public string AddressCity { get; set; } = string.Empty;
}
