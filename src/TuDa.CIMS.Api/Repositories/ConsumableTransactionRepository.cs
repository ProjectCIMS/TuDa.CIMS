﻿using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Errors;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Entities;

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

    /// <inheritdoc />
    public Task<List<ConsumableTransaction>> GetAllAsync() =>
        ConsumableTransactionsFilledQuery.ToListAsync();

    /// <inheritdoc />
    public Task<ConsumableTransaction?> GetOneAsync(Guid id) =>
        ConsumableTransactionsFilledQuery.SingleOrDefaultAsync(i => i.Id == id);

    /// <inheritdoc />
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
        int dif = newAmount - transaction.AmountChange;

        if (transaction.Consumable.Amount + dif < 0)
            return ConsumableTransactionError.AmountChangeNegative();

        if (newAmount == 0)
        {
            await RemoveAsync(consumableTransactionId);
        }
        else
        {
            consumable.Amount += dif;
            transaction.AmountChange += dif;
        }

        await _context.SaveChangesAsync();
        return Result.Updated;
    }

    /// <inheritdoc />
    public async Task<ErrorOr<Deleted>> RemoveAsync(Guid consumableTransactionId)
    {
        var transaction = await GetOneAsync(consumableTransactionId);
        if (transaction is null)
            return ConsumableTransactionError.NotFound(consumableTransactionId);

        transaction.Consumable.Amount -= transaction.AmountChange;

        _context.ConsumableTransactions.Remove(transaction);
        await _context.SaveChangesAsync();
        return Result.Deleted;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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
