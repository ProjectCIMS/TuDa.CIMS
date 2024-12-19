using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IPurchaseRepository
{
    Task<IEnumerable<Purchase>> GetAllAsync(Guid workingGroupId);
    Task<Purchase?> GetOneAsync(Guid id, Guid workingGroupId);
    Task<ErrorOr<Updated>> UpdateAsync(Guid id, Guid workingGroupId, UpdatePurchaseDto updateModel);
    Task<ErrorOr<Deleted>> RemoveAsync(Guid id, Guid workingGroupId);
    Task<ErrorOr<Created>> CreateAsync(Guid workingGroupId, CreatePurchaseDto createModel);
}
