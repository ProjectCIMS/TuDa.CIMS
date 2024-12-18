using TuDa.CIMS.Api.Models;

namespace TuDa.CIMS.Api.Interfaces;

public interface IInvoiceGenerationService
{
    public Task<ErrorOr<Invoice>> CollectInvoiceForWorkingGroup(
        Guid workingGroupId,
        DateOnly beginDate,
        DateOnly endDate
    );
}
