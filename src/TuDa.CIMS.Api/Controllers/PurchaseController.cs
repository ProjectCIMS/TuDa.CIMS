using Microsoft.AspNetCore.Mvc;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Dtos;

namespace TuDa.CIMS.Api.Controllers;

[ApiController]
[Route("api/purchases")]
public class PurchaseController : ControllerBase
{
    private readonly IPurchaseService _purchaseService;

    public PurchaseController(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    /// <summary>
    /// Retrieves all purchases from the service and returns them in an appropriate HTTP response.
    /// </summary>
    /// <param name="workingGroupId">the unique id of a workinggroup </param>
    /// <returns>a 200 OK response if the operation is successfully and a 400 BadRequest response if any error occurs </returns>
    [HttpGet($"{{{nameof(workingGroupId)}:guid}}")]
    public async Task<IActionResult> GetAllAsync(Guid workingGroupId)
    {
        return (await _purchaseService.GetAllAsync(workingGroupId)).Match<IActionResult>(
            purchases => Ok(purchases),
            error => BadRequest(error)
        );
    }

    /// <summary>
    /// Retrieves a purchase with the specific id from the service and returns it in an appropriate HTTP response.
    /// </summary>
    /// <param name="id">the unique id of the purchase</param>
    /// <param name="workingGroupId">the unique id of a workinggroup </param>
    /// <returns>a 200 OK response if the operation is successfully and a 400 BadRequest response if any error occurs  </returns>
    [HttpGet($"{{{nameof(workingGroupId)}:guid}}/{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> GetOneAsync(Guid id, Guid workingGroupId)
    {
        return (await _purchaseService.GetOneAsync(id, workingGroupId)).Match<IActionResult>(
            purchase => Ok(purchase),
            error => BadRequest(error)
        );
    }


    /// <summary>
    /// Removes a purchase with the specific id from the service.
    /// If the removal is successful, returns a 200 OK response. If an error occurs during the removal, an appropriate error response is returned.
    /// </summary>
    /// <param name="id">the unique id of the purchase</param>
    /// <param name="workingGroupId">the unique id of a workinggroup </param>
    /// <returns> a 200 OK response if the operation is successfully and a 400 BadRequest response if any error occurs </returns>
    [HttpDelete($"{{{nameof(workingGroupId)}:guid}}/{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> RemoveAsync(Guid id, Guid workingGroupId)
    {
        return (await _purchaseService.RemoveAsync(id, workingGroupId)).Match<IActionResult>(
            _ => Ok(),
            error => BadRequest(error)
        );
    }

    /// <summary>
    /// Creates a new purchase using the provided create model.
    /// If the creation is successful, returns a 200 OK response. If an error occurs during the creation, an appropriate error response is returned.
    /// </summary>
    /// <param name="createModel">the model containing the data for the new purchase</param>
    /// <param name="workingGroupId">the unique id of a workinggroup </param>
    /// <returns>a 200 OK response and the object if the operation is successfully and a 400 BadRequest response if any error occurs </returns>
    [HttpPost($"{{{nameof(workingGroupId)}:guid}}")]
    public async Task<IActionResult> CreateAsync(Guid workingGroupId, [FromBody] CreatePurchaseDto createModel)
    {
        return (await _purchaseService.CreateAsync(workingGroupId, createModel)).Match<IActionResult>(
            _ => Ok(),
            error => BadRequest(error)
        );
    }
}
