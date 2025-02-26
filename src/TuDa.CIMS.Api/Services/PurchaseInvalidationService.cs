using TuDa.CIMS.Api.Errors;
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
    private readonly IPurchaseRepository _purchaseRepository;

    public PurchaseInvalidationService(
        IConsumableTransactionRepository consumableTransactionRepository,
        IPurchaseRepository purchaseRepository
    )
    {
        _consumableTransactionRepository = consumableTransactionRepository;
        _purchaseRepository = purchaseRepository;
    }

    /// <inheritdoc />
    public async Task<ErrorOr<Updated>> UpdateForInvalidatedPurchase(
        Guid workingGroupId,
        Guid invalidatedPurchaseId,
        Guid newPurchaseId
    )
    {
        var invalidatedPurchase = await _purchaseRepository.GetOneAsync(
            workingGroupId,
            invalidatedPurchaseId
        );
        if (invalidatedPurchase is null)
            return PurchaseError.NotFound(invalidatedPurchaseId);

        var newPurchase = await _purchaseRepository.GetOneAsync(workingGroupId, newPurchaseId);
        if (newPurchase is null)
            return PurchaseError.NotFound(newPurchaseId);

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

        var deleted = await RemoveRemovedConsumableTransactions(
            invalidTransactions,
            newTransactions
        );
        if (deleted.IsError)
            return deleted.Errors;

        foreach (var item in changes)
        {
            if (invalidTransactions.TryGetValue(item.ConsumableId, out var transaction))
            {
                var success = await _consumableTransactionRepository.UpdateAmountAsync(
                    transaction.Id,
                    item.AmountChange
                );
                if (success.IsError)
                    return success.Errors;
            }
            else
            {
                var created = await CreateNewConsumableTransaction(item, newPurchase);
                if (created.IsError)
                    return created.Errors;
            }
        }

        await _consumableTransactionRepository.MoveToSuccessorPurchaseAsync(
            invalidatedPurchase.Id,
            newPurchase.Id
        );

        return Result.Updated;
    }

    private async Task<ErrorOr<Created>> CreateNewConsumableTransaction(
        CreateConsumableTransactionDto item,
        Purchase newPurchase
    )
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

        return Result.Created;
    }

    private async Task<ErrorOr<Deleted>> RemoveRemovedConsumableTransactions(
        Dictionary<Guid, ConsumableTransaction> invalidTransactions,
        List<CreateConsumableTransactionDto> newTransactions
    )
    {
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

        return Result.Deleted;
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
