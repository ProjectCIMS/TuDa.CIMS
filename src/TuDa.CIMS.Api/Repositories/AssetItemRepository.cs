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

    public async Task<IEnumerable<AssetItem>> GetAll()
    {
        return await _context.AssetItems.Include(i => i.Room).ToListAsync();
    }

    public async Task<AssetItem> GetOne(Guid id)
    {
        return await _context.AssetItems.Include(i => i.Room).Where(i => i.Id == id).SingleAsync();
    }

    public async Task Update(Guid id)
    {
        //await _context.AssetItems.Include(i => i.Room).Up
    }

    public async Task Remove(Guid id)
    {
        var itemToRemove = await _context
            .AssetItems.Include(i => i.Room)
            .Where(i => i.Id == id)
            .SingleAsync();
        _context.AssetItems.Remove(itemToRemove);
        await _context.SaveChangesAsync();
    }
}
