using TuDa.CIMS.Api.Models;
using TuDa.CIMS.Shared.Models;

namespace TuDa.CIMS.Api.Interfaces;

public interface IInvoiceGenerationService
{
    public Task<ErrorOr<Invoice>> CollectInvoiceForWorkingGroup(
        Guid workingGroupId,
        DateOnly? beginDate,
        DateOnly? endDate
    );

    public Task<ErrorOr<InvoiceStatistics>> GetInvoiceStatistics(
        Guid workingGroupId,
        DateOnly? beginDate,
        DateOnly? endDate
    );
}
