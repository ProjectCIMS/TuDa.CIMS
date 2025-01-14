using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Dtos;

public record UpdatePurchaseDto
{
    /// <summary>
    /// Buyer of the purchase.
    /// </summary>
    public Person? Buyer { get; set; }

    /// <summary>
    /// Signature of the purchase.
    /// </summary>
    public byte[]? Signature { get; set; }

    /// <summary>
    /// Entries of the purchase.
    /// </summary>
    public List<PurchaseEntry>? Entries { get; set; }

    /// <summary>
    /// Completion date of the purchase.
    /// </summary>
    public DateTime? CompletionDate { get; set; }

    /// TODO: This should be a deleted in an extra issue
    public bool? Completed { get; set; }
}
