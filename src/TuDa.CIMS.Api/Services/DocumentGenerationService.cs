using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using TuDa.CIMS.Api.Documents;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Models;

namespace TuDa.CIMS.Api.Services;

[ScopedService]
public class DocumentGenerationService : IDocumentGenerationService
{
    private readonly IInvoiceGenerationService _invoiceService;

    public DocumentGenerationService(IInvoiceGenerationService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    public async Task<ErrorOr<byte[]>> GenerateInvoice(
        Guid workingGroupId,
        AdditionalInvoiceInformation information,
        DateOnly? beginDate,
        DateOnly? endDate
    ) =>
        await _invoiceService
            .CollectInvoiceForWorkingGroup(workingGroupId, beginDate, endDate)
            .Then(invoice =>
                Document
                    .Merge(
                        new InvoiceCoverDocument(invoice, information) { },
                        new InvoiceTablesDocument(invoice)
                    )
                    .UseContinuousPageNumbers()
                    .GeneratePdf()
            );
}
