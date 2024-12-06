namespace TuDa.CIMS.Shared.Dtos;

public record UpdateGasCylinderDto : UpdateSubstanceDto
{
    /// <summary>
    /// The volume of the item.
    /// </summary>
    public required double? Volume { get; set; }

    /// <summary>
    /// The pressure of the item.
    /// </summary>
    public required double? Pressure { get; set; }
}
