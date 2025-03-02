using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IConsumableTransactionRepository
{
    Task<List<ConsumableTransaction>> GetAllAsync();
    Task<ConsumableTransaction?> GetOneAsync(Guid id);
    Task<ErrorOr<ConsumableTransaction>> CreateAsync(
        CreateConsumableTransactionDto consumableTransactionDto
    );
    Task<ErrorOr<Updated>> UpdateAmountAsync(Guid consumableTransactionId, int newAmount);
    Task<ErrorOr<Success>> MoveToSuccessorPurchaseAsync(
        Guid predecessorPurchaseId,
        Guid successorPurchaseId
    );

    /// <summary>
    /// Retrieves all consumable transactions for a specific consumable and optional year.
    /// </summary>
    /// <param name="consumableId">The unique identifier of the consumable.</param>
    /// <param name="year">The optional year to filter transactions.</param>
    Task<ErrorOr<List<ConsumableTransaction>>> GetAllOfConsumableAsync(
        Guid consumableId,
        int? year
    );

    public Task<ErrorOr<Success>> AddToPurchaseAsync(
        Guid purchaseGuid,
        Guid consumableTransactionGuid
    );
}
