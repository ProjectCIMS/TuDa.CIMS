using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Api.Services;

[ScopedService]
public class PurchaseInvalidationService : IPurchaseInvalidationService
{
    private readonly IConsumableTransactionRepository _consumableTransactionRepository;

    public PurchaseInvalidationService(
        IConsumableTransactionRepository consumableTransactionRepository
    )
    {
        _consumableTransactionRepository = consumableTransactionRepository;
    }

    public async Task<ErrorOr<Updated>> UpdateForInvalidatedPurchase(
        Purchase invalidatedPurchase,
        Purchase newPurchase
    )
    {
        var invalidTransactions = invalidatedPurchase.ConsumableTransactions.ToDictionary(x =>
            x.Consumable.Id
        );
        var newTransactions = CreateDtosFromPurchase(newPurchase).ToList();

        var changes = newTransactions.ExceptBy(
            invalidTransactions.Values.Select(entry => new
            {
                ConsumableId = entry.Consumable.Id,
                Amount = entry.AmountChange,
            }),
            entry => new { entry.ConsumableId, Amount = entry.AmountChange }
        );

        var removedAssetItems = invalidTransactions
            .Values.Select(t => t.Consumable.Id)
            .Except(newTransactions.Select(x => x.ConsumableId))
            .ToList();

        foreach (var removed in removedAssetItems)
        {
            var success = await _consumableTransactionRepository.UpdateAmountAsync(
                invalidTransactions[removed].Id,
                0
            );
            if (success.IsError)
            {
                return success.Errors;
            }
        }

        invalidatedPurchase.ConsumableTransactions.RemoveAll(x =>
            removedAssetItems.Contains(x.Consumable.Id)
        );

        foreach (var item in changes)
        {
            if (invalidTransactions.TryGetValue(item.ConsumableId, out var transaction))
            {
                var success = await _consumableTransactionRepository.UpdateAmountAsync(
                    transaction.Id,
                    item.AmountChange
                );
                if (success.IsError)
                {
                    return success.Errors;
                }
            }
            else
            {
                var newTransaction = await _consumableTransactionRepository.CreateAsync(
                    new CreateConsumableTransactionDto()
                    {
                        ConsumableId = item.ConsumableId,
                        Date = newPurchase.CompletionDate!.Value,
                        AmountChange = item.AmountChange,
                        TransactionReason = TransactionReasons.Purchase,
                    }
                );

                if (newTransaction.IsError)
                {
                    return newTransaction.Errors;
                }

                await _consumableTransactionRepository.AddToPurchaseAsync(
                    newPurchase.Id,
                    newTransaction.Value.Id
                );
            }
        }

        await _consumableTransactionRepository.MoveToSuccessorPurchaseAsync(
            invalidatedPurchase.Id,
            newPurchase.Id
        );

        return Result.Updated;
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
}
