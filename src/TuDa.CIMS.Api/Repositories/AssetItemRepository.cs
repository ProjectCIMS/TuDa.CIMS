using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Repositories;

public class AssetItemRepository : IAssetItemRepository
{
    private readonly ApplicationDbContext _context;

    public AssetItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AssetItem>> GetAll()
    {
        return await _context.AssetItems.Include(i => i.Room).ToListAsync();
    }

    public async Task AddAll(IEnumerable<AssetItem> assetItems)
    {
        foreach (var assetItem in assetItems)
        {
            _context.AssetItems.Add(assetItem);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<AssetItem> Get(Guid id)
    {
        return await _context.AssetItems.Include(i => i.Room).Where(i => i.Id == id).SingleAsync();
    }
}
