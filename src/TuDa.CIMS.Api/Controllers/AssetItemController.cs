using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Params;

namespace TuDa.CIMS.Api.Controllers;

[ApiController]
[Route("api/asset-items")]
public class AssetItemController : ControllerBase
{
    private readonly IAssetItemService _assetItemService;

    public AssetItemController(IAssetItemService assetItemService)
    {
        _assetItemService = assetItemService;
    }

    /// <summary>
    /// Retrieves all AssetItems from the service and returns them in an appropriate HTTP response.
    /// </summary>
    /// <returns> a 200 OK response if the operation is successfully and a 400 BadRequest response if any error occurs </returns>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return (await _assetItemService.GetAllAsync()).Match<IActionResult>(
            value => Ok(value),
            err => BadRequest(err)
        );
    }

    /// <summary>
    /// Retrieves an AssetItems with the specific id from the service and returns it in an appropriate HTTP response.
    /// </summary>
    /// <param name="id">the unique id of the AssetItem</param>
    /// <returns> a 200 OK response if the operation is successfully and a 400 BadRequest response if any error occurs </returns>
    [HttpGet($"{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> GetOneAsync(Guid id)
    {
        return (await _assetItemService.GetOneAsync(id)).Match<IActionResult>(
            value => Ok(JsonSerializer.Serialize(value)),
            err => BadRequest(err)
        );
    }

    /// <summary>
    /// Updates an existing AssetItem by its ID using the provided update model.
    /// If the update is successful, returns a 200 OK response. If an error occurs during the update, an appropriate error response is returned.
    /// </summary>
    /// <param name="id">the unique id of the AssetItem</param>
    /// <param name="updateModel">the model containing the updated values for the AssetItem </param>
    [HttpPut($"{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateAssetItemDto updateModel)
    {
        return (await _assetItemService.UpdateAsync(id, updateModel)).Match<IActionResult>(
            _ => Ok(),
            err => BadRequest(err)
        );
    }

    /// <summary>
    /// Deletes an existing AssetItem by its ID.
    /// If the removal is successful, returns a 200 OK response.
    /// If an error occurs during the deletion, an appropriate error response is returned.
    ///</summary>
    /// <param name="id">the unique id of the AssetItem</param>
    [HttpDelete($"{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> RemoveAsync(Guid id)
    {
        return (await _assetItemService.RemoveAsync(id)).Match<IActionResult>(
            _ => Ok(),
            err => BadRequest(err)
        );
    }

    /// <summary>
    /// Returns a paginated response of AssetItems based on the provided user parameters.
    /// If the operation is successful, returns a 200 OK response.
    /// If an error occurs during the operation, an appropriate error response is returned.
    /// </summary>
    /// <param name="userParams"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetPaginatedAsync([FromQuery] UserParams userParams)
    {
        return (await _assetItemService.GetPaginatedAsync(userParams)).Match<IActionResult>(
            value => Ok(value),
            err => BadRequest(err)
        );
    }
}
