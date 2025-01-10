using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Dtos;

public record UpdatePurchaseDto
{
    public Person? Buyer { get; set; }
    public byte[]? Signature { get; set; }
    public List<PurchaseEntry>? Entries { get; set; }
    public DateTime? CompletionDate { get; set; }
    public bool? Completed { get; set; }
}
