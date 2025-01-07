using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Dtos;

public record UpdateChemicalItemDto : UpdateAssetItemDto
{
    /// <summary>
    /// An unique identifier for the chemical.
    /// </summary>
    public string? Cas { get; set; }

    /// <summary>
    /// A list of hazards associated with the chemical.
    /// </summary>
    public List<Hazard>? Hazards { get; set; }

    /// <summary>
    /// The unit of the chemical.
    /// </summary>
    public MeasurementUnits? PriceUnit { get; set; }
}
