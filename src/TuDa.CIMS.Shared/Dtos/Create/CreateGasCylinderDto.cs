namespace TuDa.CIMS.Shared.Dtos.Create;

public class CreateGasCylinderDto : CreateSubstanceDto
{
    /// <summary>
    /// The volume of the item.
    /// </summary>
    public double Volume { get; set; }

    /// <summary>
    /// The pressure of the item.
    /// </summary>
    public double Pressure { get; set; }
}
