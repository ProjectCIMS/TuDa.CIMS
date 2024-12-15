using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IPurchaseRepository
{
    Task<IEnumerable<Purchase>> GetAllAsync();
    Task<Purchase?> GetOneAsync(Guid id);
    Task<ErrorOr<Updated>> UpdateAsync(Guid id, UpdatePurchaseDto updateModel);
    Task<ErrorOr<Deleted>> RemoveAsync(Guid id);
    Task<ErrorOr<Created>> CreateAsync(CreatePurchaseDto createModel);
}
