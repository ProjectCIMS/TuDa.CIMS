﻿using System.Text;
using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Repositories;

[ScopedService]
public class PurchaseRepository : IPurchaseRepository
{
    private readonly CIMSDbContext _context;
    private readonly IWorkingGroupRepository _workingGroupRepository;
    private readonly IPurchaseEntryRepository _purchaseEntryRepository;

    public PurchaseRepository(
        CIMSDbContext context,
        IWorkingGroupRepository workingGroupRepository,
        IPurchaseEntryRepository purchaseEntryRepository
    )
    {
        _context = context;
        _workingGroupRepository = workingGroupRepository;
        _purchaseEntryRepository = purchaseEntryRepository;
    }

    private IQueryable<Purchase> PurchaseFilledQuery(Guid workingGroupId) =>
        _context
            .WorkingGroups.Where(i => i.Id == workingGroupId)
            .SelectMany(i => i.Purchases)
            .Include(i => i.Buyer)
            .Include(i => i.Successor)
            .Include(i => i.Predecessor)
            .Include(i => i.ConsumableTransactions)
            .Include(i => i.Entries)
            .ThenInclude(i => i.AssetItem);

    private IQueryable<Purchase> PurchaseEmptyQuery(Guid workingGroupId) =>
        _context.WorkingGroups.Where(i => i.Id == workingGroupId).SelectMany(i => i.Purchases);

    /// <summary>
    /// Returns all purchases from the database.
    /// </summary>
    /// <param name="workingGroupId">the specific ID of the workinggroup</param>
    /// <returns></returns>
    public Task<List<Purchase>> GetAllAsync(Guid workingGroupId) =>
        PurchaseFilledQuery(workingGroupId).ToListAsync();

    /// <summary>
    /// Returns a single purchase by its specific ID.
    /// </summary>
    /// <param name="id">the specific ID for the purchase</param>
    /// <param name="workingGroupId">the specific id of a workinggroup</param>
    /// <returns></returns>
    public Task<Purchase?> GetOneAsync(Guid workingGroupId, Guid id) =>
        PurchaseFilledQuery(workingGroupId).Where(p => p.Id == id).SingleOrDefaultAsync();

    /// <summary>
    /// Removes a purchase with the specific ID from the database.
    /// </summary>
    /// <param name="id">the specific ID of the purchase</param>
    /// <param name="workingGroupId">the specific ID of a workinggroup</param>
    /// <returns></returns>
    public async Task<ErrorOr<Deleted>> RemoveAsync(Guid workingGroupId, Guid id)
    {
        var itemToRemove = await GetOneAsync(workingGroupId, id);

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
        var workingGroup = await _workingGroupRepository.GetOneAsync(workingGroupId);

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

        var purchaseEntries = await _purchaseEntryRepository.CreateMultipleAsync(
            createModel.Entries
        );

        if (purchaseEntries.IsError)
            return purchaseEntries.Errors;

        var newPurchase = new Purchase
        {
            Buyer = buyer,
            Signature = createModel.Signature,
            CompletionDate = createModel.CompletionDate,
            Entries = purchaseEntries.Value,
        };

        workingGroup.Purchases.Add(newPurchase);
        await _context.SaveChangesAsync();

        return newPurchase;
    }

    /// <inheritdoc />
    public async Task<ErrorOr<Success>> SetSuccessorAndPredecessorAsync(
        Guid workingGroupId,
        Guid predecessorId,
        Guid successorId
    )
    {
        var predecessor = await GetOneAsync(workingGroupId, predecessorId);
        if (predecessor is null)
        {
            return Error.NotFound(
                "Purchase.SetSuccessorAndPredecessorAsync",
                $"Purchase with id {predecessorId} was not found."
            );
        }

        var successor = await GetOneAsync(workingGroupId, successorId);
        if (successor is null)
        {
            return Error.NotFound(
                "Purchase.SetSuccessorAndPredecessorAsync",
                $"Purchase with id {successorId} was not found."
            );
        }

        predecessor.Successor = successor;
        successor.Predecessor = predecessor;

        await _context.SaveChangesAsync();
        return Result.Success;
    }

    /// <inheritdoc />
    public async Task<ErrorOr<string>> RetrieveSignatureAsync(Guid workingGroupId, Guid purchaseId)
    {
        var purchase = await GetOneAsync(workingGroupId, purchaseId);
        if (purchase is null)
        {
            return Error.NotFound(
                "Purchase.RetrieveSignature",
                $"Purchase with id {purchaseId} was not found."
            );
        }
        byte[] signature = purchase.Signature;
        return Encoding.UTF8.GetString(signature);
    }
}
