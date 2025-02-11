using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Extensions;

public static class PurchaseEntryExtension
{
    public static CreatePurchaseEntryDto ToCreateDto(this PurchaseEntry entry) =>
        new()
        {
            AssetItemId = entry.AssetItem.Id,
            Amount = entry.Amount,
            PricePerItem = entry.PricePerItem,
        };
}
