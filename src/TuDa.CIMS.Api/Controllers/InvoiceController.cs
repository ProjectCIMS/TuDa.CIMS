using Microsoft.AspNetCore.Mvc;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared;
using TuDa.CIMS.Shared.Models;

namespace TuDa.CIMS.Api.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoiceController : CIMSBaseController
{
    private readonly IInvoiceGenerationService _invoiceService;
    private readonly IDocumentGenerationService _documentService;

    public InvoiceController(
        IInvoiceGenerationService invoiceService,
        IDocumentGenerationService documentService
    )
    {
        _invoiceService = invoiceService;
        _documentService = documentService;
    }

    [HttpGet($"{{{nameof(workingGroupId)}:guid}}/statistics")]
    public async Task<IActionResult> GetInvoiceStatistics(
        Guid workingGroupId,
        [FromQuery] DateOnly? beginDate,
        [FromQuery] DateOnly? endDate
    ) =>
        (await _invoiceService.GetInvoiceStatistics(workingGroupId, beginDate, endDate)).Match(
            onValue: Ok,
            onError: ErrorsToProblem
        );

    [HttpPost($"{{{nameof(workingGroupId)}:guid}}/document")]
    public async Task<IActionResult> GetInvoiceDocument(
        [FromRoute] Guid workingGroupId,
        [FromBody] AdditionalInvoiceInformation additionalInformation,
        [FromQuery] DateOnly? beginDate,
        [FromQuery] DateOnly? endDate
    ) =>
        (
            await _documentService.GenerateInvoice(
                workingGroupId,
                additionalInformation,
                beginDate,
                endDate
            )
        ).Match(
            onValue: pdf => File(pdf, "application/pdf", "invoice.pdf"),
            onError: ErrorsToProblem
        );
}
