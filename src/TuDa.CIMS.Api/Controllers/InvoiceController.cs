using Microsoft.AspNetCore.Mvc;
using TuDa.CIMS.Api.Interfaces;

namespace TuDa.CIMS.Api.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceGenerationService _invoiceService;

    public InvoiceController(IInvoiceGenerationService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet($"{{{nameof(workingGroupId)}:guid}}/statistics")]
    public async Task<IActionResult> GetInvoiceStatistics(
        Guid workingGroupId,
        [FromQuery] DateOnly? beginDate,
        [FromQuery] DateOnly? endDate
    ) =>
        (
            await _invoiceService.GetInvoiceStatistics(workingGroupId, beginDate, endDate)
        ).Match<IActionResult>(Ok, BadRequest);
}
