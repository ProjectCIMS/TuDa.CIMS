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
        var room = new Room { Id = Guid.NewGuid(), Name = "Raum" };
        List<AssetItem> items =
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
        ];
        return Ok(items);
    }

    [HttpGet($"{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok(
            new Consumable
            {
                Manufacturer = "fjeifje",
                SerialNumber = "fgjiefoefr",
                Id = Guid.NewGuid(),
                Room = new Room { Id = Guid.NewGuid(), Name = "Raum" },
                Name = "jiofej",
                ItemNumber = "gjikoefe",
                Shop = "jfikoefj",
            }
        );
    }

    [HttpPut]
    public async Task<IActionResult> Create(AssetItem item)
    {
        return Ok(Guid.NewGuid());
    }

    [HttpDelete($"{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> Remove(Guid id)
    {
        return Ok();
    }

    [HttpPatch($"{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> Update(Guid id, AssetItem item)
    {
        return Ok(item with { Id = id });
    }
}
