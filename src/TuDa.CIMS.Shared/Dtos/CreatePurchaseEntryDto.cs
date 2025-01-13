using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Dtos;

public record CreatePurchaseEntryDto
{
    /// <summary>
    /// The purchased asset item.
    /// TODO: Could change AssetItem to Guid and load the entity in the repository.
    /// </summary>
    public required Guid AssetItem { get; set; }

    /// <summary>
    /// The amount of purchased items.
    /// </summary>
    public required int Amount { get; set; }

    /// <summary>
    /// The current item price when the purchase is done.
    /// </summary>
    public required double PricePerItem { get; set; }
}
