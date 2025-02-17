using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Models;

namespace TuDa.CIMS.Api.Services;

[ScopedService]
public class ConsumableService : IConsumableService
{
    private readonly IConsumableTransactionRepository _transactionRepository;
    private readonly IAssetItemRepository _assetItemRepository;

    public ConsumableService(
        IConsumableTransactionRepository transactionRepository,
        IAssetItemRepository assetItemRepository
    )
    {
        _transactionRepository = transactionRepository;
        _assetItemRepository = assetItemRepository;
    }

    /// <inheritdoc />
    public async Task<ErrorOr<ConsumableStatistics>> GetStatistics(Guid consumableId, int year)
    {
        try
        {
            var assetItem = await _assetItemRepository.GetOneAsync(consumableId);
            if (assetItem is not Consumable consumable)
            {
                return Error.NotFound(
                    "Consumable.NotFound",
                    $"A consumable with id {consumableId} was not found."
                );
            }

            var transactions = await _transactionRepository.GetAllOfConsumableAsync(
                consumableId,
                year
            );
            if (transactions.IsError)
            {
                return transactions.Errors;
            }

            var statistics = TransactionsToStatistics(transactions.Value);

            statistics.CurrentAmount = consumable.Amount;
            statistics.PreviousYearAmount =
                statistics.CurrentAmount - statistics.TotalAdded + statistics.TotalRemoved;

            return statistics;
        }
        catch (Exception e)
        {
            return Error.Failure(e.GetType().Name, e.Message);
        }
    }

    /// <summary>
    /// Converts a list of consumable transactions into consumable statistics.
    /// </summary>
    /// <param name="transactions">The list of consumable transactions.</param>
    /// <returns>The consumable statistics.</returns>
    private static ConsumableStatistics TransactionsToStatistics(
        List<ConsumableTransaction> transactions
    ) =>
        transactions.Aggregate(
            new ConsumableStatistics(),
            (statistics, transaction) =>
            {
                if (transaction.AmountChange > 0)
                {
                    statistics.TotalAdded += transaction.AmountChange;
                }
                else
                {
                    statistics.TotalRemoved -= transaction.AmountChange;
                }

                return statistics;
            }
        );
}
