using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IPurchaseService
{
    Task<ErrorOr<IEnumerable<Purchase>>> GetAllAsync();
    Task<ErrorOr<Purchase>> GetOneAsync(Guid id);
    Task<ErrorOr<Updated>> UpdateAsync(Guid id, UpdatePurchaseDto updateModel);
    Task<ErrorOr<Deleted>> RemoveAsync(Guid id);
    Task<ErrorOr<Created>> CreateAsync(CreatePurchaseDto createModel);
}
