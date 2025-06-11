namespace TuDa.CIMS.Shared.Dtos.Update;

public record UpdateChemicalDto : UpdateSubstanceDto
{
    /// <summary>
    /// The binding size of the item.
    /// </summary>
    public double? BindingSize { get; set; }
}
