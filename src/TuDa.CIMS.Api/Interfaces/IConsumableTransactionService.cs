using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IConsumableTransactionService
{
    Task<ErrorOr<List<ConsumableTransaction>>> GetAllAsync();
    Task<ErrorOr<ConsumableTransaction>> GetOneAsync(Guid id);
    Task<ErrorOr<Created>> CreateAsync(CreateConsumableTransactionDto consumableTransactionDto);
    Task<ErrorOr<Created>> CreateForPurchaseAsync(Purchase purchase);
}
