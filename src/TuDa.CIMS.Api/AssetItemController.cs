using Microsoft.AspNetCore.Mvc;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api;

[ApiController]
[Route("api/asset-items")]
public class AssetItemController : ControllerBase
{
    private readonly IAssetItemService _assetItemService;
    private readonly IAssetItemRepository _assetItemRepository;
    private readonly ApplicationDbContext _applicationDbContext;

    public AssetItemController(
        IAssetItemService assetItemService,
        IAssetItemRepository assetItemRepository,
        ApplicationDbContext applicationDbContext
    )
    {
        _assetItemService = assetItemService;
        _assetItemRepository = assetItemRepository;
        _applicationDbContext = applicationDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _assetItemService.GetAll());
    }

    [HttpPut]
    public async Task<IActionResult> AddSome()
    {
        var room = new Room { Id = Guid.NewGuid(), Name = "Raum" };
        _applicationDbContext.Rooms.Add(room);
        await _applicationDbContext.SaveChangesAsync();
        await _assetItemRepository.AddAll(
            [
                new Chemical
                {
                    Cas = "12343",
                    Unit = "mg",
                    Id = Guid.NewGuid(),
                    Room = room,
                    Name = "Chemikalien",
                    ItemNumber = "erwwref",
                    Shop = "fjfoej",
                },
                new Consumable
                {
                    Manufacturer = "fjeifje",
                    SerialNumber = "fgjiefoefr",
                    Id = Guid.NewGuid(),
                    Room = room,
                    Name = "jiofej",
                    ItemNumber = "gjikoefe",
                    Shop = "jfikoefj",
                },
            ]
        );
        return Ok();
    }
}
