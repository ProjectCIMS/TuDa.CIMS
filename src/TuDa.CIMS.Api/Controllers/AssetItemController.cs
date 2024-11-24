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

    public async Task<IActionResult> GetAll()
    {
        //return (await _assetItemService.GetAll()).Match
    }

    public async Task<IActionResult> GetOne(Guid id)
    {
        return (await _assetItemService.GetOne(id)).Match<IActionResult>(
            value => Ok(value),
            err => BadRequest(err)
        );
    }
}
