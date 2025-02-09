using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IPurchaseRepository
{
    Task<List<Purchase>> GetAllAsync(Guid workingGroupId);
    Task<Purchase?> GetOneAsync(Guid workingGroupId, Guid id);
    Task<ErrorOr<Deleted>> RemoveAsync(Guid workingGroupId, Guid id);
    Task<ErrorOr<Purchase>> CreateAsync(Guid workingGroupId, CreatePurchaseDto createModel);

    /// <summary>
    /// Invalidate a purchase and correct it with a new one.
    /// </summary>
    /// <param name="workingGroupId">The id of the workingGroup of the purchase.</param>
    /// <param name="purchaseId">The id of the purchase to invalidate.</param>
    /// <param name="createModel">The purchase to correct the old one.</param>
    public Task<ErrorOr<Success>> InvalidateAsync(
        Guid workingGroupId,
        Guid purchaseId,
        CreatePurchaseDto createModel
    );
}
