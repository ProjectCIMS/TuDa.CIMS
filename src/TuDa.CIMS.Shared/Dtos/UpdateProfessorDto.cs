namespace TuDa.CIMS.Shared.Dtos;

public record UpdateProfessorDto : UpdatePersonDto
{
    /// <summary>
    /// The title of the professor.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The street of the professor.
    /// </summary>
    public string? AddressStreet { get; set; }

    /// <summary>
    /// The number of the street.
    /// </summary>
    public int? AddressNumber { get; set; }

    /// <summary>
    /// The zip code of the city.
    /// </summary>
    public string? AddressZipCode { get; set; }

    /// <summary>
    /// The city of the professors address.
    /// </summary>
    public string? AddressCity { get; set; }
}
