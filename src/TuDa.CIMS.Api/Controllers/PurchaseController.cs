using Microsoft.AspNetCore.Mvc;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Controllers;

[ApiController]
[Route("api/purchase")]
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
    /// <returns>a 200 OK response if the operation is successfully and a 400 BadRequest response if any error occurs </returns>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return (await _purchaseService.GetAllAsync()).Match<IActionResult>(
            purchases => Ok(purchases),
            error => BadRequest(error)
        );
    }

    /// <summary>
    /// Retrieves a purchase with the specific id from the service and returns it in an appropriate HTTP response.
    /// </summary>
    /// <param name="id">the unique id of the purchase</param>
    /// <returns>a 200 OK response if the operation is successfully and a 400 BadRequest response if any error occurs  </returns>
    [HttpGet($"{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> GetOneAsync(Guid id)
    {
        return (await _purchaseService.GetOneAsync(id)).Match<IActionResult>(
            purchase => Ok(purchase),
            error => BadRequest(error)
        );
    }

    /// <summary>
    /// Updates an existing purchase by its ID using the provided update model.
    /// If the update is successful, returns a 200 OK response. If an error occurs during the update, an appropriate error response is returned.
    /// </summary>
    /// <param name="id">the unique id of the purchase</param>
    /// <param name="updateModel">the model containing the updated values for the purchase </param>
    /// <returns> a 200 OK response if the operation is successfully and a 400 BadRequest response if any error occurs </returns>
    [HttpPatch($"{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdatePurchaseDto updateModel)
    {
        return (await _purchaseService.UpdateAsync(id, updateModel)).Match<IActionResult>(
            updated => Ok(updated),
            error => BadRequest(error)
        );
    }

    /// <summary>
    /// Removes a purchase with the specific id from the service.
    /// If the removal is successful, returns a 200 OK response. If an error occurs during the removal, an appropriate error response is returned.
    /// </summary>
    /// <param name="id">the unique id of the purchase</param>
    /// <returns> a 200 OK response if the operation is successfully and a 400 BadRequest response if any error occurs </returns>
    [HttpDelete($"{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> RemoveAsync(Guid id)
    {
        return (await _purchaseService.RemoveAsync(id)).Match<IActionResult>(
            _ => Ok(_),
            error => BadRequest(error)
        );
    }

    /// <summary>
    /// Creates a new purchase using the provided create model.
    /// If the creation is successful, returns a 200 OK response. If an error occurs during the creation, an appropriate error response is returned.
    /// </summary>
    /// <param name="createModel">the model containing the data for the new purchase</param>
    /// <returns>a 200 OK response and the object if the operation is successfully and a 400 BadRequest response if any error occurs </returns>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePurchaseDto createModel)
    {
        return (await _purchaseService.CreateAsync(createModel)).Match<IActionResult>(
            created => Ok(created),
            error => BadRequest(error)
        );
    }
}
