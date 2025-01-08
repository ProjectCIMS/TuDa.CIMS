﻿namespace TuDa.CIMS.Shared.Entities;

public class Address
{
    /// <summary>
    /// The street of the professor.
    /// </summary>
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// The number of the street.
    /// </summary>
    public int Number { get; set; } = 0;

    /// <summary>
    /// The zip code of the city.
    /// </summary>
    public string ZipCode { get; set; } = string.Empty;

    /// <summary>
    /// The city of the professors address.
    /// </summary>
    public string City { get; set; } = string.Empty;
}
