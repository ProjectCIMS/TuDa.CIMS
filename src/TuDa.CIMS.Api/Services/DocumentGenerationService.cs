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
    private readonly HttpClient _httpClient;

    /// <summary>
    /// This is the Url of the TuDa logo. This needs to be a png or jpg.
    /// </summary>
    private readonly string _logoUrl;

    private Image? _logo { get; set; }

    public DocumentGenerationService(
        IConfiguration configuration,
        IInvoiceGenerationService invoiceService,
        HttpClient httpClient
    )
    {
        _logoUrl =
            configuration["TuDaLogoUrl"]
            ?? throw new ArgumentException("TuDaLogoUrl need to be provided");
        _invoiceService = invoiceService;
        _httpClient = httpClient;
    }

    private async Task<Image?> GetLogo()
    {
        if (_logo is not null)
            return _logo;

        var response = await _httpClient.GetAsync(_logoUrl);
        _logo = response.IsSuccessStatusCode
            ? Image.FromBinaryData(await response.Content.ReadAsByteArrayAsync())
            : null;

        return _logo;
    }

    public async Task<ErrorOr<byte[]>> GenerateInvoice(
        Guid workingGroupId,
        AdditionalInvoiceInformation information,
        DateOnly? beginDate,
        DateOnly? endDate
    ) =>
        await _invoiceService
            .CollectInvoiceForWorkingGroup(workingGroupId, beginDate, endDate)
            .ThenAsync(async invoice =>
                Document
                    .Merge(
                        new InvoiceCoverDocument(invoice, await GetLogo(), information) { },
                        new InvoiceTablesDocument(invoice)
                    )
                    .UseContinuousPageNumbers()
                    .GeneratePdf()
            );
}
