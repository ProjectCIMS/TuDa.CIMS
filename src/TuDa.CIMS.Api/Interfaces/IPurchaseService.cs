using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IPurchaseService
{
    Task<ErrorOr<IEnumerable<Purchase>>> GetAllAsync(Guid workingGroupId);
    Task<ErrorOr<Purchase>> GetOneAsync(Guid id, Guid workingGroupId);
    Task<ErrorOr<Deleted>> RemoveAsync( Guid id, Guid workingGroupId);
    Task<ErrorOr<Created>> CreateAsync(Guid workingGroupId, CreatePurchaseDto createModel);
     Task<ErrorOr<Created>> CreateForPurchaseAsync(Purchase purchase);
}
