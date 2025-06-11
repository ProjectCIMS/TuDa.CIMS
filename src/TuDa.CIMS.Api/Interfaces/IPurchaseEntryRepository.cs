using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IPurchaseEntryRepository
{
    public Task<ErrorOr<List<PurchaseEntry>>> CreateMultipleAsync(
        List<CreatePurchaseEntryDto> createEntries
    );
}
