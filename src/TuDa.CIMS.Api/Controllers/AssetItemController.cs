using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Entities;

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
        return (await _assetItemService.GetAllAsync())
            .ToErrorOr()
            .Match<IActionResult>(
                value => Ok(JsonSerializer.Serialize(value)),
                err => BadRequest(JsonSerializer.Serialize(err))
            );
    }

    /// <summary>
    /// Retrieves an AssetItems with the specific id from the service and returns it in an appropriate HTTP response.
    /// </summary>
    /// <param name="id">the unique id of the AssetItem</param>
    /// <returns> a 200 OK response if the operation is successfully and a 400 BadRequest response if any error occurs </returns>
    [HttpGet]
    public async Task<IActionResult> GetOneAsync(Guid id)
    {
        return (await _assetItemService.GetOneAsync(id)).Match<IActionResult>(
            value => Ok(JsonSerializer.Serialize(value)),
            err => BadRequest(JsonSerializer.Serialize(err))
        );
    }

    /// <summary>
    /// Calls the <see cref="IAssetItemService.UpdateAsync"/> function of the service.
    /// </summary>
    /// <param name="id">the unique id of the AssetItem</param>
    /// <param name="updateModel">the model containing the updated values for the AssetItem </param>
    /// <returns> a 200 OK response </returns>

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(Guid id, AssetItem updateModel)
    {
        await _assetItemService.UpdateAsync(id, updateModel);
        return Ok();
    }

    /// <summary>
    /// Calls the <see cref="IAssetItemService.RemoveAsync"/> function of the service.
    /// If the update is successful, returns a 200 OK response.
    ///</summary>
    /// <param name="id">the unique id of the AssetItem</param>
    /// <returns> a 200 OK response </returns>

    [HttpDelete]
    public async Task<IActionResult> RemoveAsync(Guid id)
    {
        await _assetItemService.RemoveAsync(id);
        return Ok();
    }
}
