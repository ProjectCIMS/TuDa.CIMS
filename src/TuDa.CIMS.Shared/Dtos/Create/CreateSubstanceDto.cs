﻿using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Dtos.Create;

public class CreateSubstanceDto : CreateAssetItemDto
{
    /// <summary>
    /// A unique identifier for the chemical.
    /// </summary>
    public string Cas { get; set; } = string.Empty;

    /// <summary>
    /// The purity of the item.
    /// </summary>
    public string Purity { get; set; } = string.Empty;

    /// <summary>
    /// The price unit of the item.
    /// </summary>
    public MeasurementUnits PriceUnit { get; set; }

    /// <summary>
    /// A list of hazards associated with the chemical.
    /// </summary>
    public List<Guid> Hazards { get; set; } = [];
}
