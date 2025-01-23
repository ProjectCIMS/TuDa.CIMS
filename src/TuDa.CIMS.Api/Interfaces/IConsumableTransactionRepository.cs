using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IConsumableTransactionRepository
{
    Task<List<ConsumableTransaction>> GetAllAsync();
    Task<ConsumableTransaction?> GetOneAsync(Guid id);
    Task<ErrorOr<Created>> CreateAsync(CreateConsumableTransactionDto consumableTransactionDto);
}
