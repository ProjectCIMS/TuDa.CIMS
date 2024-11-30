using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Repositories;

public class AssetItemRepository : IAssetItemRepository
{
    private readonly CIMSDbContext _context;

    public AssetItemRepository(CIMSDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Returns all existing AssetItems of the database.
    /// </summary>
    public async Task<IEnumerable<AssetItem>> GetAllAsync()
    {
        return await _context.AssetItems.Include(i => i.Room).ToListAsync();
    }

    /// <summary>
    /// Returns an existing AssetItem with the specific id.
    /// </summary>
    /// <param name="id">the unique id of the AssetItem</param>
    public async Task<AssetItem?> GetOneAsync(Guid id)
    {
        return await _context.AssetItems.Where(i => i.Id == id).Include(i => i.Room).SingleAsync();
    }

    public Task<ErrorOr<Updated>> UpdateAsync(Guid id, AssetItem updateModel) =>
        throw new NotImplementedException();

    /// <summary>
    /// Updates an existing AssetItem with the specified ID using the provided update model.
    /// </summary>
    /// <param name="id">the unique id of the AssetItem</param>
    /// <param name="updateModel">the model containing the updated values for the AssetItem </param>
    public async Task<ErrorOr<Updated>> UpdateAsync(Guid id, UpdateAssetItemDto updateModel)
    {
        var existingItem = await _context
            .AssetItems.Include(i => i.Room)
            .SingleOrDefaultAsync(i => i.Id == id);

        if (existingItem is null)
        {
            return Error.NotFound();
        }

        existingItem.ItemNumber = updateModel.ItemNumber ?? existingItem.Name;
        existingItem.Note = updateModel.Note ?? existingItem.Name;
        existingItem.Shop = updateModel.Shop ?? existingItem.Name;
        existingItem.Name = updateModel.Name ?? existingItem.Name;

        switch (existingItem, updateModel)
        {
            case (Chemical chemical, UpdateChemicalItemDto update):
                chemical.Cas = update.Cas ?? chemical.Cas;
                chemical.Hazards = update.Hazards ?? chemical.Hazards;
                chemical.Unit = update.Unit ?? chemical.Unit;
                break;
            case (Consumable consumable, UpdateConsumableItemDto update):
                consumable.Manufacturer = update.Manufacturer ?? consumable.Manufacturer;
                consumable.SerialNumber = update.SerialNumber ?? consumable.SerialNumber;
                break;
        }

        if (existingItem.Room is null)
        {
            return Error.NotFound(
                "Assetitem.update",
                $"Room id of AssetItem with id {id} not found."
            );
        }
        var room = await _context.Rooms.SingleOrDefaultAsync(r => r.Id == updateModel.RoomId);
        existingItem.Room.Id = updateModel.RoomId ?? existingItem.Room.Id;

        await _context.SaveChangesAsync();
        return Result.Updated;
    }

    ///Removes an AssetItem with the specific id from the database.
    /// </summary>
    /// <param name="id">the unique id of the AssetItem</param>
    public async Task<ErrorOr<Deleted>> RemoveAsync(Guid id)
    {
        var itemToRemove = await _context
            .AssetItems.Include(i => i.Room)
            .Where(i => i.Id == id)
            .SingleAsync();
        _context.AssetItems.Remove(itemToRemove);

        await _context.SaveChangesAsync();
        return Result.Deleted;
    }
}
