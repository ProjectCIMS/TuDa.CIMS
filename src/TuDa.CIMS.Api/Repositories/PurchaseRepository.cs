using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Repositories;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly CIMSDbContext _context;

    public PurchaseRepository(CIMSDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Returns all purchases from the database.
    /// </summary>
    /// <param name="workingGroupId">the specific ID of the workinggroup</param>
    /// <returns></returns>
    public async Task<IEnumerable<Purchase>> GetAllAsync(Guid workingGroupId)
    {
        return await _context
            .WorkingGroups.Where(i => i.Id == workingGroupId)
            .SelectMany(i => i.Purchases)
            .Include(i => i.Buyer)
            .Include(i => i.Entries)
            .ThenInclude(i => i.AssetItem)
            .ToListAsync();
    }

    /// <summary>
    /// Returns a single purchase by its specific ID.
    /// </summary>
    /// <param name="id">the specific ID for the purchase</param>
    /// <param name="workingGroupId">the specific id of a workinggroup</param>
    /// <returns></returns>
    public async Task<Purchase?> GetOneAsync(Guid id, Guid workingGroupId)
    {
        return await _context
            .WorkingGroups.Where(i => i.Id == workingGroupId)
            .SelectMany(i => i.Purchases)
            .Where(i => i.Id == id)
            .Include(i => i.Buyer)
            .Include(i => i.Entries)
            .ThenInclude(i => i.AssetItem)
            .SingleOrDefaultAsync();
    }

    /// <summary>
    /// Removes a purchase with the specific ID from the database.
    /// </summary>
    /// <param name="id">the specific ID of the purchase</param>
    /// <param name="workingGroupId">the specific ID of a workinggroup</param>
    /// <returns></returns>
    public async Task<ErrorOr<Deleted>> RemoveAsync(Guid id, Guid workingGroupId)
    {
        var itemToRemove = await _context
            .WorkingGroups.Where(i => i.Id == workingGroupId)
            .SelectMany(i => i.Purchases)
            .Where(i => i.Id == id)
            .SingleOrDefaultAsync();

        if (itemToRemove is null)
        {
            return Error.NotFound("Purchase.remove", $"Purchase with ID {id} was not found.");
        }

        _context.Purchases.Remove(itemToRemove);

        await _context.SaveChangesAsync();
        return Result.Deleted;
    }

    /// <summary>
    /// Creates a new purchase in the database.
    /// </summary>
    /// <param name="workingGroupId">the specific ID of a workinggroup</param>
    /// <param name="createModel">the model containing the updated values for the purchase</param>
    /// <returns></returns>
    public async Task<ErrorOr<Purchase>> CreateAsync(
        Guid workingGroupId,
        CreatePurchaseDto createModel
    )
    {
        var workingGroup = await _context
            .WorkingGroups.Include(i => i.Purchases)
            .SingleOrDefaultAsync(r => r.Id == workingGroupId);

        if (workingGroup is null)
        {
            return Error.NotFound(
                "Purchase.create",
                $"WorkingGroup with ID {workingGroupId} was not found."
            );
        }

        var buyer = await _context.Persons.SingleOrDefaultAsync(r => r.Id == createModel.Buyer);

        if (buyer is null)
        {
            return Error.NotFound(
                "Purchase.create",
                $"Person with ID {createModel.Buyer} was not found."
            );
        }

        var assetItemIds = createModel.Entries.Select(e => e.AssetItemId).Distinct().ToList();
        var assetItems = await _context
            .AssetItems.Where(ai => assetItemIds.Contains(ai.Id))
            .ToDictionaryAsync(ai => ai.Id);

        var newPurchase = new Purchase
        {
            Buyer = buyer,
            Signature = createModel.Signature,
            CompletionDate = createModel.CompletionDate,
            Completed = true,
            Entries =
                createModel
                    .Entries?.Select(e => new PurchaseEntry
                    {
                        AssetItem = assetItems.TryGetValue(e.AssetItemId, out var assetItem)
                            ? assetItem
                            : throw new Exception($"AssetItem with ID {e.AssetItemId} not found."),
                        Amount = e.Amount,
                        PricePerItem = e.PricePerItem,
                    })
                    .ToList() ?? [],
        };

        workingGroup.Purchases.Add(newPurchase);
        await _context.SaveChangesAsync();

        return newPurchase;
    }
}
