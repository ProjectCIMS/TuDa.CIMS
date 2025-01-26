using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Repositories;

[ScopedService]
public class PurchaseEntryRepository : IPurchaseEntryRepository
{
    private readonly CIMSDbContext _context;

    public PurchaseEntryRepository(CIMSDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<List<PurchaseEntry>>> CreateMultipleAsync(
        List<CreatePurchaseEntryDto> createEntries
    )
    {
        var assetItemIds = createEntries.Select(e => e.AssetItemId).Distinct().ToList();
        var assetItems = await _context
            .AssetItems.Where(ai => assetItemIds.Contains(ai.Id))
            .ToDictionaryAsync(ai => ai.Id);

        List<PurchaseEntry> purchaseEntries = [];

        foreach (var entry in createEntries)
        {
            if (!assetItems.TryGetValue(entry.AssetItemId, out var assetItem))
            {
                return Error.NotFound(
                    "AssetItem.NotFound",
                    $"AssetItem with id {entry.AssetItemId} could not be found"
                );
            }
            purchaseEntries.Add(
                new PurchaseEntry
                {
                    AssetItem = assetItem,
                    Amount = entry.Amount,
                    PricePerItem = entry.PricePerItem,
                }
            );
        }

        await _context.PurchaseEntries.AddRangeAsync(purchaseEntries);
        await _context.SaveChangesAsync();

        return purchaseEntries;
    }
}
