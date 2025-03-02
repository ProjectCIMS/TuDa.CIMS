namespace TuDa.CIMS.Shared.Entities;

/// <summary>
/// An entity representing a purchase in the system.
/// </summary>
public record Purchase : BaseEntity
{
    /// <summary>
    /// The person that purchase the items.
    /// </summary>
    public required Person Buyer { get; set; }

    /// <summary>
    /// The signature of the buyer.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public byte[] Signature { get; set; } = [];

    /// <summary>
    /// All entries of the purchase.
    /// </summary>
    public List<PurchaseEntry> Entries { get; set; } = [];

    /// <summary>
    /// The date of completion.
    /// </summary>
    public DateTime? CompletionDate { get; set; }

    #region Invalidation

    /// <summary>
    /// If the purchase is invalidated to correct it with another one.
    /// </summary>
    public bool Invalidated => Successor is not null;

    /// <summary>
    /// Not null when Purchase is invalidated.
    /// The purchase this purchase is corrected with.
    /// </summary>
    public Purchase? Successor { get; set; }

    /// <summary>
    /// The purchase this purchase is correcting.
    /// </summary>
    public Purchase? Predecessor { get; set; }

    /// <summary>
    /// All for the Purchase created Consumable transaction
    /// </summary>
    public List<ConsumableTransaction> ConsumableTransactions { get; init; } = [];

    #endregion

    #region Methods

    /// <summary>
    /// Calculates the TotalPrice of this purchase.
    /// </summary>
    public double TotalPrice => Entries.Aggregate(0.0, (total, entry) => total + entry.TotalPrice);

    #endregion
}
