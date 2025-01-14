using TuDa.CIMS.Shared.Models;

namespace TuDa.CIMS.Api.Interfaces;

public interface IDocumentGenerationService
{
    public Task<ErrorOr<byte[]>> GenerateInvoice(
        Guid workingGroupId,
        AdditionalInvoiceInformation information,
        DateOnly? beginDate,
        DateOnly? endDate
    );
}
