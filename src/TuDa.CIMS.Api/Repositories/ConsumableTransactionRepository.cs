using Microsoft.EntityFrameworkCore;
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

    public async Task<ErrorOr<Updated>> UpdateForInvalidatedPurchase(
        Purchase invalidatedPurchase,
        Purchase newPurchase
    )
    {
        var invalidTransactions = invalidatedPurchase.ConsumableTransactions.ToDictionary(x =>
            x.Consumable.Id
        );

        var changes = newPurchase
            .Entries.Where(entry => entry.AssetItem is Consumable)
            .ExceptBy(
                invalidatedPurchase.Entries.Select(entry => new
                {
                    entry.AssetItem.Id,
                    Amount = (int)entry.Amount,
                }),
                entry => new { entry.AssetItem.Id, Amount = (int)entry.Amount }
            );

        var changedGrouped = changes.GroupBy(e => e.AssetItem.Id).ToList();

        var removedAssetItems = invalidatedPurchase
            .ConsumableTransactions.Select(t => t.Consumable.Id)
            .Except(newPurchase.Entries.Select(x => x.AssetItem.Id).Distinct());

        invalidatedPurchase.ConsumableTransactions.RemoveAll(x =>
            removedAssetItems.Contains(x.Consumable.Id)
        );

        foreach (var item in changedGrouped)
        {
            if (invalidTransactions.TryGetValue(item.Key, out var transaction))
            {
                var success = await UpdateAmountAsync(
                    transaction.Id,
                    (int)-item.Sum(x => x.Amount)
                );
                if (success.IsError)
                {
                    return success.Errors;
                }
            }
            else
            {
                var newTransaction = await CreateAsync(
                    new CreateConsumableTransactionDto()
                    {
                        ConsumableId = item.Key,
                        Date = newPurchase.CompletionDate!.Value,
                        AmountChange = (int)-item.Sum(x => x.Amount),
                        TransactionReason = TransactionReasons.Purchase,
                    }
                );

                if (newTransaction.IsError)
                {
                    return newTransaction.Errors;
                }

                newPurchase.ConsumableTransactions.Add(newTransaction.Value);
            }
        }

        await _context.SaveChangesAsync();
        await MoveToSuccessorPurchaseAsync(invalidatedPurchase.Id, newPurchase.Id);

        return Result.Updated;
    }

    /// <inheritdoc />
    public async Task<ErrorOr<Updated>> UpdateAmountAsync(
        Guid consumableTransactionId,
        int newAmount
    )
    {
        var transaction = await GetOneAsync(consumableTransactionId);
        if (transaction is null)
        {
            return ConsumableTransactionError.NotFound(consumableTransactionId);
        }

        var consumable = transaction.Consumable;

        if (transaction.Consumable.Amount + newAmount < 0)
        {
            return ConsumableTransactionError.AmountChangeNegative();
        }

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
}
