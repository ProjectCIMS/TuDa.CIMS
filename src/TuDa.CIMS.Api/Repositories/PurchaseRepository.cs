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
        return await _context.WorkingGroups
            .Where(i => i.Id == workingGroupId)
            .SelectMany(i => i.Purchases)
            .Include(i => i.Buyer)
            .Include(i => i.Entries)
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
        return await _context.WorkingGroups
            .Where(i => i.Id == workingGroupId)
            .SelectMany(i => i.Purchases)
            .Where(i => i.Id == id)
            .Include(i => i.Buyer)
            .Include(i => i.Entries)
            .SingleOrDefaultAsync();
    }

    /// <summary>
    /// Updates an existing purchase by its ID using the provided update model.
    /// </summary>
    /// <param name="id">the specific ID of the purchase</param>
    /// <param name="workingGroupId">the specific id of a workinggroup</param>
    /// <param name="updateModel">the model containing the update values for the purchase </param>
    /// <returns></returns>
    public async Task<ErrorOr<Updated>> UpdateAsync(Guid id, Guid workingGroupId, UpdatePurchaseDto updateModel)
    {
        var existingItem = await _context
            .WorkingGroups
            .Where(i => i.Id == workingGroupId)
            .SelectMany(i => i.Purchases)
            .Where(i => i.Id == id)
            .Include(i => i.Buyer)
            .Include(i => i.Entries)
            .SingleOrDefaultAsync();

        if (existingItem is null)
        {
            return Error.NotFound("Purchase.update", $"Purchase with ID {id} was not found.");
        }


        if (updateModel.Buyer is not null)
        {
            var buyer = await _context.Persons.SingleOrDefaultAsync(r => r.Id == updateModel.Buyer.Id);
            if (buyer is null)
            {
                return Error.NotFound("Purchase.update", $"Person with ID {updateModel.Buyer.Id} was not found.");
            }

            existingItem.Buyer = buyer;
        }

        if (updateModel.Entries is not null)
        {
            var entries = await _context.PurchaseEntries
                .Where(x => updateModel.Entries.Select(y => y.Id).Contains(x.Id)).ToListAsync();
            if (entries.Count != updateModel.Entries.Count)
            {
                return Error.NotFound("Purchase.update", $"Some of the PurchaseEntries were not found.");
            }

            existingItem.Entries = entries;
        }

        existingItem.Buyer = updateModel.Buyer ?? existingItem.Buyer;
        existingItem.Signature = updateModel.Signature ?? existingItem.Signature;
        existingItem.Entries = updateModel.Entries ?? existingItem.Entries;
        existingItem.CompletionDate = updateModel.CompletionDate ?? existingItem.CompletionDate;
        existingItem.Completed = updateModel.Completed ?? existingItem.Completed;


        await _context.SaveChangesAsync();

        return new Updated();
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
            .WorkingGroups
            .Where(i => i.Id == workingGroupId)
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
    public async Task<ErrorOr<Created>> CreateAsync(Guid workingGroupId, CreatePurchaseDto createModel)
    {
        var workingGroup = await _context.WorkingGroups.SingleOrDefaultAsync(r => r.Id == workingGroupId);
        if (workingGroup is null)
        {
            return Error.NotFound("Purchase.create",
                $"WorkingGroup with ID {workingGroupId} was not found.");
        }

        var newPurchase = new Purchase
        {
            Buyer = createModel.Buyer,
            Signature = createModel.Signature,
            CompletionDate = createModel.CompletionDate,
            Entries = createModel.Entries,
            Completed = createModel.Completed
        };

        workingGroup.Purchases.Add(newPurchase);
        await _context.SaveChangesAsync();

        return new Created();
    }
}
