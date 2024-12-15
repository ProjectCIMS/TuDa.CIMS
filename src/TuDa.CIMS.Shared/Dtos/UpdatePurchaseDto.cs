using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Dtos;

public abstract record UpdatePurchaseDto
{
    public WorkingGroup? WorkingGroup { get; set; }
    public Person? Buyer { get; set; }
    public byte[]? Signature { get; set; }
    public List<PurchaseEntry>? Entries { get; set; }
}
