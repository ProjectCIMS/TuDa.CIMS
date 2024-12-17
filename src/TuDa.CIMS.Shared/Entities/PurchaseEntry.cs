namespace TuDa.CIMS.Shared.Entities;

public record PurchaseEntry : BaseEntity
{
    /// <summary>
    /// The purchased asset item.
    /// </summary>
    public required AssetItem AssetItem { get; set; }

    /// <summary>
    /// The amount of purchased items.
    /// </summary>
    public required int Amount { get; set; }

    /// <summary>
    /// The current item price when the purchase is done.
    /// </summary>
    public required double PricePerItem { get; set; }

    #region Methods

    /// <summary>
    /// Calculates the TotalPrice of this purchase entry.
    /// </summary>
    public double TotalPrice => PricePerItem * Amount;

    #endregion
}
