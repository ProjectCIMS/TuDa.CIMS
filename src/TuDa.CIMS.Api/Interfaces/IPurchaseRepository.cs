using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IPurchaseRepository
{
    Task<List<Purchase>> GetAllAsync(Guid workingGroupId);
    Task<Purchase?> GetOneAsync(Guid id, Guid workingGroupId);
    Task<ErrorOr<Deleted>> RemoveAsync(Guid id, Guid workingGroupId);
    Task<ErrorOr<Purchase>> CreateAsync(Guid workingGroupId, CreatePurchaseDto createModel);
}
