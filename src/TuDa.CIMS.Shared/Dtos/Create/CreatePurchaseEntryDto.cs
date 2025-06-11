namespace TuDa.CIMS.Shared.Dtos.Create;

public record CreatePurchaseEntryDto
{
    /// <summary>
    /// The purchased asset item.
    /// </summary>
    public required Guid AssetItemId { get; set; }

    /// <summary>
    /// The amount of purchased items.
    /// </summary>
    public required double Amount { get; set; }

    /// <summary>
    /// The current item price when the purchase is done.
    /// </summary>
    public required double PricePerItem { get; set; }
}
