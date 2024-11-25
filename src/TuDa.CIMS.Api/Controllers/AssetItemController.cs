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

    public async Task<IActionResult> GetAllAsync()
    {
        return (await _assetItemService.GetAllAsync())
            .ToErrorOr()
            .Match<IActionResult>(value => Ok(value), err => BadRequest(err));
    }

    public async Task<IActionResult> GetOneAsync(Guid id)
    {
        return (await _assetItemService.GetOneAsync(id)).Match<IActionResult>(
            value => Ok(value),
            err => BadRequest(err)
        );
    }

    public async Task<IActionResult> UpdateAsync(Guid id, AssetItem updateModel)
    {
        return (await _assetItemService.UpdateAsync(id, updateModel)).Match<IActionResult>(
            value => Ok(value),
            err => BadRequest(err)
        );
    }
}
