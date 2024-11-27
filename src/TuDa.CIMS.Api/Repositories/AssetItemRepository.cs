using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Repositories;

public class AssetItemRepository : IAssetItemRepository
{
    private readonly CIMSDbContext _context;

    public AssetItemRepository(CIMSDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AssetItem>> GetAllAsync()
    {
        return await _context.AssetItems.Include(i => i.Room).ToListAsync();
    }

    public async Task<AssetItem> GetOneAsync(Guid id)
    {
        return await _context.AssetItems.Include(i => i.Room).Where(i => i.Id == id).SingleAsync();
    }

    public async Task UpdateAsync(Guid id, AssetItem updateModel)
    {
        var existingItem = await _context
            .AssetItems.Include(i => i.Room)
            .SingleOrDefaultAsync(i => i.Id == id);

        existingItem.Id = updateModel.Id;
        existingItem.ItemNumber = updateModel.ItemNumber;
        existingItem.Note = updateModel.Note;
        existingItem.Shop = updateModel.Shop;

        var room = await _context.Rooms.SingleOrDefaultAsync(r => r.Id == updateModel.Room.Id);
        existingItem.Room = updateModel.Room;

        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid id)
    {
        var itemToRemove = await _context
            .AssetItems.Include(i => i.Room)
            .Where(i => i.Id == id)
            .SingleAsync();
        _context.AssetItems.Remove(itemToRemove);

        await _context.SaveChangesAsync();
    }
}
