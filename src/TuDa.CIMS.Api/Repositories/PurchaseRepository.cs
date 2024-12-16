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
    /// <returns></returns>
    public async Task<IEnumerable<Purchase>> GetAllAsync()
    {
        return await _context.Purchases
            .Include(i => i.WorkingGroup)
            .Include(x => x.Entries)
            .ToListAsync();
    }

    /// <summary>
    /// Returns a single purchase by its specific ID.
    /// </summary>
    /// <param name="id">teh specific ID for the purchase</param>
    /// <returns></returns>
    public async Task<Purchase?> GetOneAsync(Guid id)
    {
        return await _context
            .Purchases
            .Where(i => i.Id == id)
            .Include(i => i.WorkingGroup)
            .Include(x => x.Entries)
            .Include(x => x.Buyer)
            .SingleOrDefaultAsync();
    }

    /// <summary>
    /// Updates an existing purchase by its ID using the provided update model.
    /// </summary>
    /// <param name="id">the specific ID of the purchase</param>
    /// <param name="updateModel">the model containing the update values for the purchase </param>
    /// <returns></returns>
    public async Task<ErrorOr<Updated>> UpdateAsync(Guid id, UpdatePurchaseDto updateModel)
    {
        var existingItem = await _context
            .Purchases
            .Where(i => i.Id == id)
            .Include(purchase => purchase.WorkingGroup)
            .Include(y => y.Entries)
            .Include(y => y.Buyer)
            .SingleOrDefaultAsync();

        if (existingItem is null)
        {
            return Error.NotFound("Purchase.update", $"Purchase with ID {id} was not found.");
        }

        if (updateModel.WorkingGroup is not null)
        {
            var workingGroup =
                await _context.WorkingGroups.SingleOrDefaultAsync(r => r.Id == updateModel.WorkingGroup.Id);
            if (workingGroup is null)
            {
                return Error.NotFound("Purchase.update",
                    $"WorkingGroup with ID {updateModel.WorkingGroup.Id} was not found.");
            }

            existingItem.WorkingGroup = workingGroup;
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

        existingItem.WorkingGroup = updateModel.WorkingGroup ?? existingItem.WorkingGroup;
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
    /// <returns></returns>
    public async Task<ErrorOr<Deleted>> RemoveAsync(Guid id)
    {
        var itemToRemove = await _context
            .Purchases
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
    /// <param name="createModel">the model containing the updated values for the purchase</param>
    /// <returns></returns>
    public async Task<ErrorOr<Created>> CreateAsync(CreatePurchaseDto createModel)
    {
        var workingGroup = await _context.WorkingGroups.SingleOrDefaultAsync(r => r.Id == createModel.WorkingGroup.Id);
        if (workingGroup is null)
        {
            return Error.NotFound("Purchase.create",
                $"WorkingGroup with ID {createModel.WorkingGroup.Id} was not found.");
        }

        var newPurchase = new Purchase
        {
            WorkingGroup = createModel.WorkingGroup,
            Buyer = createModel.Buyer,
            Signature = createModel.Signature,
            CompletionDate = createModel.CompletionDate,
            Entries = createModel.Entries,
            Completed = createModel.Completed
        };

        await _context.Purchases.AddAsync(newPurchase);
        await _context.SaveChangesAsync();

        return new Created();
    }
}
