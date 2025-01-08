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

    private readonly Image _logo;

    public DocumentGenerationService(IInvoiceGenerationService invoiceService)
    {
        _invoiceService = invoiceService;
        _logo = Image.FromFile("tuda_logo.png");
    }

    public Task<ErrorOr<byte[]>> GenerateInvoice(
        Guid workingGroupId,
        AdditionalInvoiceInformation information,
        DateOnly? beginDate,
        DateOnly? endDate
    ) =>
        _invoiceService
            .CollectInvoiceForWorkingGroup(workingGroupId, beginDate, endDate)
            .Then(invoice =>
                Document
                    .Merge(
                        new InvoiceCoverDocument(invoice, _logo, information) { },
                        new InvoiceTablesDocument(invoice)
                    )
                    .UseContinuousPageNumbers()
                    .GeneratePdf()
            );
}
