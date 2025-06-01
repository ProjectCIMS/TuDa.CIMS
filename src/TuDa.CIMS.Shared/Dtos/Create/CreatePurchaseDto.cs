namespace TuDa.CIMS.Shared.Dtos.Create;

public record CreatePurchaseDto
{
    /// <summary>
    /// The person that purchase the items.
    /// </summary>
    public required Guid Buyer { get; set; }

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
    public List<CreatePurchaseEntryDto> Entries { get; set; } = [];

    /// <summary>
    /// The date of completion.
    /// </summary>
    public DateTime? CompletionDate { get; set; }
}
