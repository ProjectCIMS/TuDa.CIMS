using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IConsumableTransactionService
{
    Task<ErrorOr<List<ConsumableTransaction>>> GetAllAsync();
    Task<ErrorOr<ConsumableTransaction>> GetOneAsync(Guid id);
    Task<ErrorOr<ConsumableTransaction>> CreateAsync(
        CreateConsumableTransactionDto consumableTransactionDto
    );
    Task<ErrorOr<Created>> CreateForPurchaseAsync(Purchase purchase);
}
