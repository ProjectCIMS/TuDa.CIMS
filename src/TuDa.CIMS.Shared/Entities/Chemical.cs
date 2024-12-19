namespace TuDa.CIMS.Shared.Entities;

/// <summary>
/// An entity representing a chemical.
/// </summary>
public record Chemical : Substance
{
    /// <summary>
    /// The binding size of the chemical.
    /// </summary>
    public double BindingSize { get; set; }
}
