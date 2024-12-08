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
        return await _context
            .AssetItems.Where(i => i.Id == id)
            .Include(i => i.Room)
            .SingleOrDefaultAsync();
    }

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
            return Error.NotFound("Assetitem.update", $"AssetItem with ID {id} was not found.");
        }

        existingItem.ItemNumber = updateModel.ItemNumber ?? existingItem.ItemNumber;
        existingItem.Note = updateModel.Note ?? existingItem.Note;
        existingItem.Shop = updateModel.Shop ?? existingItem.Shop;
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
            default:
                return Error.Failure(
                    "Assetitem.update",
                    $"The provided update model does not match the type of the AssetItem with ID {id}."
                );
        }

        if (updateModel.RoomId is not null)
        {
            var room = await _context.Rooms.SingleOrDefaultAsync(r => r.Id == updateModel.RoomId);
            if (room is null)
            {
                return Error.NotFound(
                    "Assetitem.update",
                    $"Given RoomId {updateModel.RoomId} was not found."
                );
            }
            existingItem.Room = room;
        }

        await _context.SaveChangesAsync();
        return Result.Updated;
    }

    /// <summary>
    ///Removes an AssetItem with the specific id from the database.
    /// </summary>
    /// <param name="id">the unique id of the AssetItem</param>
    public async Task<ErrorOr<Deleted>> RemoveAsync(Guid id)
    {
        var itemToRemove = await _context
            .AssetItems.Include(i => i.Room)
            .Where(i => i.Id == id)
            .SingleOrDefaultAsync();

        if (itemToRemove is null)
        {
            return Error.NotFound(
                "Assetitem.remove",
                $"The asset item with the id {id} was not found."
            );
        }
        _context.AssetItems.Remove(itemToRemove);

        await _context.SaveChangesAsync();
        return Result.Deleted;
    }

    public async Task<IEnumerable<AssetItem>> SearchAsync(string name)
    {
        return await _context
            .AssetItems.Include(i => i.Room)
            .Where(i => EF.Functions.Like(i.Name, $"%{name}%"))
            .ToListAsync();
    }
}
