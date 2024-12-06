using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Dtos;

public record UpdateChemicalDto : UpdateSubstanceDto
{
    /// <summary>
    /// The binding size of the item.
    /// </summary>
    public double? BindingSize { get; set; }
}
