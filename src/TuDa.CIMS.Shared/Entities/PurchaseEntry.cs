namespace TuDa.CIMS.Shared.Entities;

public record PurchaseEntry(AssetItem AssetItem, int Amount) : BaseEntity
{
    /// <summary>
    /// The purchased asset item.
    /// </summary>
    public required AssetItem AssetItem { get; set; } = AssetItem;

    /// <summary>
    /// The amount of purchased items.
    /// </summary>
    public required int Amount { get; set; } = Amount;

    /// <summary>
    /// The current item price when the purchase is done.
    /// </summary>
    public double PricePerItem { get; set; } = AssetItem.Price;

    #region Methods

    /// <summary>
    /// Calculates the TotalPrice of this purchase entry.
    /// </summary>
    public double TotalPrice => PricePerItem * Amount;

    #endregion
}
