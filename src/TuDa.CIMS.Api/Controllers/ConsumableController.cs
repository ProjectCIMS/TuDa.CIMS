using Microsoft.AspNetCore.Mvc;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared;

namespace TuDa.CIMS.Api.Controllers;

[ApiController]
[Route("api/consumables")]
public class ConsumableController : CIMSBaseController
{
    private readonly IConsumableService _service;

    public ConsumableController(IConsumableService service)
    {
        _service = service;
    }

    [HttpGet($"{{{nameof(consumableId)}}}/statistics/{{{nameof(year)}}}")]
    [ProducesResponseType(200)]
    public Task<IActionResult> GetStatisticsForYear(Guid consumableId, int year) =>
        _service.GetStatistics(consumableId, year).Match(onValue: Ok, onError: ErrorsToProblem);
}
