namespace TuDa.CIMS.Shared.Entities;

/// <summary>
/// An entity representing a gas.
/// </summary>
public record GasCylinder : Substance
{
    /// <summary>
    /// The volume of the item.
    /// </summary>
    public required double Volume { get; set; }

    /// <summary>
    /// The pressure of the item.
    /// </summary>
    public required double Pressure { get; set; }
};
