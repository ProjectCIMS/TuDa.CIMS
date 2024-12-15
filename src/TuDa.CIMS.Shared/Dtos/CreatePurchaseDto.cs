using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Dtos;

public abstract record CreatePurchaseDto
{
    /// <summary>
    /// The working group that purchased the items.
    /// </summary>
    public required WorkingGroup WorkingGroup { get; set; }

    /// <summary>
    /// The person that purchase the items.
    /// </summary>
    public required Person Buyer { get; set; }

    /// <summary>
    /// The signature of the buyer.
    /// </summary>
    /// <remarks>
    /// TODO: The saving strategy is not final.
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

    /// <summary>
    /// If the purchase is actually completed.
    /// </summary>
    public bool Completed { get; set; }
}
