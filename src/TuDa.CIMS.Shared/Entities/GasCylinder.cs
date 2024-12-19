namespace TuDa.CIMS.Shared.Entities;

/// <summary>
/// An entity representing a gas cylinder.
/// </summary>
public record GasCylinder : Substance
{
    /// <summary>
    /// The volume of the cylinder.
    /// </summary>
    public double Volume { get; set; }

    /// <summary>
    /// The pressure of the cylinder.
    /// </summary>
    public double Pressure { get; set; }
}
