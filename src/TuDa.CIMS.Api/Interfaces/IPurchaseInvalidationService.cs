using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IPurchaseInvalidationService
{
    public Task<ErrorOr<Updated>> UpdateForInvalidatedPurchase(
        Purchase invalidatedPurchase,
        Purchase newPurchase
    );
}
