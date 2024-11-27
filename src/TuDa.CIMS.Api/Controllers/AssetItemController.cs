﻿using System.Text.Json;
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

    [HttpGet]
    public async Task<IActionResult> GetOneAsync(Guid id)
    {
        return (await _assetItemService.GetOneAsync(id)).Match<IActionResult>(
            value => Ok(JsonSerializer.Serialize(value)),
            err => BadRequest(JsonSerializer.Serialize(err))
        );
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(Guid id, AssetItem updateModel)
    {
        await _assetItemService.UpdateAsync(id, updateModel);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveAsync(Guid id)
    {
        await _assetItemService.RemoveAsync(id);
        return Ok();
    }
}
