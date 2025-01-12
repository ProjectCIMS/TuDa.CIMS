using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IConsumableTransactionService
{
    Task<ErrorOr<IEnumerable<ConsumableTransaction>>> GetAllAsync();
    Task<ErrorOr<ConsumableTransaction>> GetOneAsync(Guid id);
    Task<ErrorOr<Created>> CreateAsync(CreateConsumableTransactionDto ConsumableTransactionDto);
    Task<ErrorOr<Created>> CreateForPurchaseAsync(Purchase purchase);
}
