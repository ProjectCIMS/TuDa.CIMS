﻿using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Errors;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Api.Repositories;

[ScopedService]
public class ConsumableTransactionRepository : IConsumableTransactionRepository
{
    private readonly CIMSDbContext _context;

    public ConsumableTransactionRepository(CIMSDbContext context)
    {
        _context = context;
    }

    private IQueryable<ConsumableTransaction> ConsumableTransactionsFilledQuery =>
        _context.ConsumableTransactions.Include(ct => ct.Consumable);

    /// <summary>
    /// Returns all existing AssetItems of the database.
    /// </summary>
    public Task<List<ConsumableTransaction>> GetAllAsync() =>
        ConsumableTransactionsFilledQuery.ToListAsync();

    /// <summary>
    /// Returns an existing ConsumableTransaction with the specific id.
    /// </summary>
    /// <param name="id">the unique id of the ConsumableTransaction</param>
    public Task<ConsumableTransaction?> GetOneAsync(Guid id) =>
        ConsumableTransactionsFilledQuery.SingleOrDefaultAsync(i => i.Id == id);

    /// <summary>
    /// Updates the amount of the specific Consumable of the ConsumableTransaction and creates a new transaction for a consumable item.
    /// </summary>
    /// <param name="consumableTransactionDto"></param>
    public async Task<ErrorOr<ConsumableTransaction>> CreateAsync(
        CreateConsumableTransactionDto consumableTransactionDto
    )
    {
        var consumable = await _context.Consumables.SingleOrDefaultAsync(i =>
            i.Id == consumableTransactionDto.ConsumableId
        );

        if (consumable is null)
        {
            return ConsumableError.NotFound(consumableTransactionDto.ConsumableId);
        }

        if (consumable.Amount + consumableTransactionDto.AmountChange < 0)
        {
            return ConsumableTransactionError.AmountChangeNegative();
        }

        consumable.Amount += consumableTransactionDto.AmountChange;

        ConsumableTransaction consumableTransaction =
            new()
            {
                Date = consumableTransactionDto.Date,
                Consumable = consumable,
                AmountChange = consumableTransactionDto.AmountChange,
                TransactionReason = consumableTransactionDto.TransactionReason,
            };

        _context.ConsumableTransactions.Add(consumableTransaction);
        await _context.SaveChangesAsync();

        return consumableTransaction;
    }

    /// <inheritdoc />
    public async Task<ErrorOr<Updated>> UpdateAmountAsync(
        Guid consumableTransactionId,
        int newAmount
    )
    {
        var transaction = await GetOneAsync(consumableTransactionId);
        if (transaction is null)
            return ConsumableTransactionError.NotFound(consumableTransactionId);

        var consumable = transaction.Consumable;

        if (transaction.Consumable.Amount + newAmount < 0)
            return ConsumableTransactionError.AmountChangeNegative();

        consumable.Amount -= transaction.AmountChange - newAmount;

        if (newAmount == 0)
        {
            _context.ConsumableTransactions.Remove(transaction);
        }
        else
        {
            transaction.AmountChange = newAmount;
        }

        await _context.SaveChangesAsync();
        return Result.Updated;
    }

    public async Task<ErrorOr<Success>> MoveToSuccessorPurchaseAsync(
        Guid predecessorPurchaseId,
        Guid successorPurchaseId
    )
    {
        var predecessor = await _context
            .Purchases.Include(p => p.ConsumableTransactions)
            .SingleOrDefaultAsync(p => p.Id == predecessorPurchaseId);

        if (predecessor is null)
            return PurchaseError.NotFound(predecessorPurchaseId);

        var successor = await _context
            .Purchases.Include(p => p.ConsumableTransactions)
            .SingleOrDefaultAsync(p => p.Id == successorPurchaseId);

        if (successor is null)
            return PurchaseError.NotFound(successorPurchaseId);

        successor.ConsumableTransactions.AddRange(predecessor.ConsumableTransactions);
        predecessor.ConsumableTransactions.Clear();

        await _context.SaveChangesAsync();
        return Result.Success;
    }

    /// <inheritdoc />
    public async Task<ErrorOr<List<ConsumableTransaction>>> GetAllOfConsumableAsync(
        Guid consumableId,
        int? year
    )
    {
        if (!await _context.Consumables.AnyAsync(consumable => consumable.Id == consumableId))
        {
            return ConsumableError.NotFound(consumableId);
        }

        var transactionsQuery = ConsumableTransactionsFilledQuery.Where(transaction =>
            transaction.Consumable.Id == consumableId
        );

        if (year is not null)
        {
            transactionsQuery = transactionsQuery.Where(transaction =>
                transaction.Date.Year == year
            );
        }

        return await transactionsQuery.ToListAsync();
    }

    private static IEnumerable<CreateConsumableTransactionDto> CreateDtosFromPurchase(
        Purchase purchase
    ) =>
        purchase
            .Entries.Where(e => e.AssetItem is Consumable)
            .GroupBy(entry => entry.AssetItem.Id)
            .Select(group => new CreateConsumableTransactionDto()
            {
                ConsumableId = group.First().AssetItem.Id,
                Date = purchase.CompletionDate!.Value,
                AmountChange = (int)-group.Sum(entry => entry.Amount),
                TransactionReason = TransactionReasons.Purchase,
            });

    public async Task<ErrorOr<Success>> AddToPurchaseAsync(
        Guid purchaseGuid,
        Guid consumableTransactionGuid
    )
    {
        var purchase = await _context
            .Purchases.Include(p => p.ConsumableTransactions)
            .SingleOrDefaultAsync(p => p.Id == purchaseGuid);
        if (purchase is null)
        {
            return PurchaseError.NotFound(purchaseGuid);
        }

        var consumableTransaction = await GetOneAsync(consumableTransactionGuid);
        if (consumableTransaction is null)
        {
            return ConsumableTransactionError.NotFound(consumableTransactionGuid);
        }

        purchase.ConsumableTransactions.Add(consumableTransaction);
        await _context.SaveChangesAsync();
        return Result.Success;
    }
}
