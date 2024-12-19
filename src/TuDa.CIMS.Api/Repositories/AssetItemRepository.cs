using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Params;
using TuDa.CIMS.Api.Factories;

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
        existingItem.Price = updateModel.Price ?? existingItem.Price;

        switch (existingItem, updateModel)
        {
            case (Solvent solvent, UpdateSolventDto update):
                solvent.Cas = update.Cas ?? solvent.Cas;
                solvent.Hazards = update.Hazards ?? solvent.Hazards;
                solvent.PriceUnit = update.PriceUnit ?? solvent.PriceUnit;
                solvent.BindingSize = update.BindingSize ?? solvent.BindingSize;
                solvent.Purity = update.Purity ?? solvent.Purity;
                break;

            case (Chemical chemical, UpdateChemicalDto update):
                chemical.Cas = update.Cas ?? chemical.Cas;
                chemical.Hazards = update.Hazards ?? chemical.Hazards;
                chemical.PriceUnit = update.PriceUnit ?? chemical.PriceUnit;
                chemical.BindingSize = update.BindingSize ?? chemical.BindingSize;
                chemical.Purity = update.Purity ?? chemical.Purity;
                break;

            case (GasCylinder gas, UpdateGasCylinderDto update):
                gas.Cas = update.Cas ?? gas.Cas;
                gas.Hazards = update.Hazards ?? gas.Hazards;
                gas.PriceUnit = update.PriceUnit ?? gas.PriceUnit;
                gas.Volume = update.Volume ?? gas.Volume;
                gas.Pressure = update.Pressure ?? gas.Pressure;
                gas.Purity = update.Purity ?? gas.Purity;
                break;

            case (Consumable consumable, UpdateConsumableDto update):
                consumable.Amount = update.Amount ?? consumable.Amount;
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
    /// Removes an AssetItem with the specific id from the database.
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

    /// <summary>
    /// Returns a paginated list of AssetItems.
    /// </summary>
    /// <param name="userParams"></param>
    /// <returns></returns>
    public async Task<ErrorOr<PaginatedResponse<AssetItem>>> GetPaginatedAsync(AssetItemPaginationQueryParams queryParams)
    {
        var query = _context.AssetItems.AsQueryable();
        return await PaginatedResponseFactory<AssetItem>.CreateAsync(query, queryParams.PageNumber, queryParams.PageSize);

    ///Returns a list of matching AssetItem based on the provided name or CAS number.
    /// </summary>
    /// <param name="nameOrCas"></param>
    public async Task<IEnumerable<AssetItem>> SearchAsync(string nameOrCas)
    {
        IQueryable<AssetItem> query;

        bool isCas = nameOrCas.All(c => char.IsDigit(c) || c == '-');

        if (isCas)
        {
            query = _context.Substances.Where(s => EF.Functions.ILike(s.Cas, $"{nameOrCas}%")).Include(s => s.Hazards);
        }
        else
        {
            query = _context.AssetItems.Where(i => EF.Functions.ILike(i.Name, $"{nameOrCas}%"));
        }

        return await query.Include(i => i.Room).ToListAsync();
    }
}
