namespace TuDa.CIMS.Shared.Entities;

public record Address
{
    /// <summary>
    /// City of the address.
    /// </summary>
    public string City { get; set; } = "Darmstadt";

    /// <summary>
    /// Zip code of the city.
    /// </summary>
    public string ZipCode { get; set; } = "64387";

    /// <summary>
    /// Street of the address.
    /// </summary>
    public required string Street { get; set; }

    /// <summary>
    /// House number of the address.
    /// </summary>
    public required string HouseNumber { get; set; }
}
