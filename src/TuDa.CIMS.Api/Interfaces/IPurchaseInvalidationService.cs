using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IPurchaseInvalidationService
{
    public Task<ErrorOr<Updated>> UpdateForInvalidatedPurchase(
        Guid workingGroupId,
        Guid invalidatedPurchaseId,
        Guid newPurchaseId
    );
}
