using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IPurchaseService
{
    Task<ErrorOr<List<Purchase>>> GetAllAsync(Guid workingGroupId);
    Task<ErrorOr<Purchase>> GetOneAsync(Guid workingGroupId, Guid id);
    Task<ErrorOr<Deleted>> RemoveAsync(Guid workingGroupId, Guid id);
    Task<ErrorOr<Purchase>> CreateAsync(Guid workingGroupId, CreatePurchaseDto createModel);
}
