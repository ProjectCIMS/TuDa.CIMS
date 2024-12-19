namespace TuDa.CIMS.Shared.Dtos;

public record UpdateGasCylinderDto : UpdateSubstanceDto
{
    /// <summary>
    /// The volume of the item.
    /// </summary>
    public double? Volume { get; set; }

    /// <summary>
    /// The pressure of the item.
    /// </summary>
    public double? Pressure { get; set; }
}
