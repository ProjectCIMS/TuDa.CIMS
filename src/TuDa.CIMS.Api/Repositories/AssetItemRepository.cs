using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Factories;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Shared.Params;

namespace TuDa.CIMS.Api.Repositories;

[ScopedService]
public class AssetItemRepository : IAssetItemRepository
{
    private readonly CIMSDbContext _context;

    public AssetItemRepository(CIMSDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Query for all <see cref="AssetItem"/>s from the DB with included properties.
    /// </summary>
    private IQueryable<AssetItem> AssetItemsFilledQuery => _context.AssetItems.Include(i => i.Room);

    /// <summary>
    /// Query for all <see cref="Substance"/>s from the DB with included properties.
    /// </summary>
    private IQueryable<Substance> SubstancesFilledQuery =>
        AssetItemsFilledQuery.OfType<Substance>().Include(s => s.Hazards);

    /// <summary>
    /// Query for all <see cref="GasCylinder"/>s from the DB with included properties.
    /// </summary>
    private IQueryable<GasCylinder> GasCylindersFilledQuery =>
        SubstancesFilledQuery.OfType<GasCylinder>();

    /// <summary>
    /// Query for all <see cref="Solvent"/>s from the DB with included properties.
    /// </summary>
    private IQueryable<Solvent> SolventsFilledQuery => SubstancesFilledQuery.OfType<Solvent>();

    /// <summary>
    /// Query for all <see cref="Chemical"/>s from the DB with included properties.
    /// </summary>
    private IQueryable<Chemical> ChemicalsFilledQuery => SubstancesFilledQuery.OfType<Chemical>();

    /// <summary>
    /// Query for all <see cref="Consumable"/>s from the DB with included properties.
    /// </summary>
    private IQueryable<Consumable> ConsumablesFilledQuery =>
        AssetItemsFilledQuery.OfType<Consumable>();

    /// <summary>
    /// Returns all existing AssetItems of the database.
    /// </summary>
    public Task<List<AssetItem>> GetAllAsync() => AssetItemsFilledQuery.ToListAsync();

    /// <summary>
    /// Returns an existing AssetItem with the specific id.
    /// </summary>
    /// <param name="id">the unique id of the AssetItem</param>
    public Task<AssetItem?> GetOneAsync(Guid id) =>
        AssetItemsFilledQuery.SingleOrDefaultAsync(item => item.Id == id);

    /// <summary>
    /// Updates an existing AssetItem with the specified ID using the provided update model.
    /// </summary>
    /// <param name="id">the unique id of the AssetItem</param>
    /// <param name="updateModel">the model containing the updated values for the AssetItem </param>
    public async Task<ErrorOr<Updated>> UpdateAsync(Guid id, UpdateAssetItemDto updateModel)
    {
        var existingItem = await GetOneAsync(id);

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
            case (Chemical chemical, UpdateChemicalDto update): // Solvent is handled by this
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
        var itemToRemove = await GetOneAsync(id);

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
    /// <param name="queryParams"></param>
    /// <returns></returns>
    public async Task<ErrorOr<PaginatedResponse<AssetItem>>> GetPaginatedAsync(
        AssetItemPaginationQueryParams queryParams
    )
    {
        if (
            queryParams.PageSize * (queryParams.PageNumber - 1)
            >= await _context.AssetItems.CountAsync()
        )
            return Error.Validation(
                $"{nameof(AssetItemRepository)}.{nameof(GetPaginatedAsync)}",
                "Requested Page would be empty"
            );

        return await PaginatedResponseFactory<AssetItem>.CreateAsync(
            AssetItemsFilledQuery,
            queryParams.PageNumber,
            queryParams.PageSize
        );
    }

    /// <summary>
    /// Returns a list of matching AssetItem based on the provided name or CAS number.
    /// </summary>
    /// <param name="nameOrCas"></param>
    /// <param name="assetItemTypes"></param>
    public async Task<List<AssetItem>> SearchAsync(
        string nameOrCas,
        List<AssetItemType>? assetItemTypes
    )
    {
        bool isCas = nameOrCas.All(c => char.IsDigit(c) || c == '-');

        if (assetItemTypes is not null && assetItemTypes.Count > 0)
        {
            return await FilterForAssetItemTypes(assetItemTypes, isCas, nameOrCas);
        }

        var searchQuery = isCas
            ? SubstancesFilledQuery.Where(s => EF.Functions.ILike(s.Cas, $"{nameOrCas}%"))
            : AssetItemsFilledQuery.Where(i => EF.Functions.ILike(i.Name, $"{nameOrCas}%"));
        return await searchQuery.ToListAsync();
    }

    private async Task<List<AssetItem>> FilterForAssetItemTypes(
        List<AssetItemType> assetItemTypes,
        bool isCas,
        string nameOrCas
    )
    {
        List<AssetItem> result = new List<AssetItem>();

        if (isCas)
        {
            if (assetItemTypes.Contains(AssetItemType.Chemical))
            {
                var chemicals = await ChemicalsFilledQuery
                    .Where(c => c.GetType() == typeof(Chemical))
                    .Where(c => EF.Functions.ILike(c.Cas, $"{nameOrCas}%"))
                    .ToListAsync();
                result.AddRange(chemicals);
            }

            if (assetItemTypes.Contains(AssetItemType.Solvent))
            {
                var solvents = await SolventsFilledQuery
                    .Where(s => EF.Functions.ILike(s.Cas, $"{nameOrCas}%"))
                    .ToListAsync();
                result.AddRange(solvents);
            }
        }
        else
        {
            if (assetItemTypes.Contains(AssetItemType.Chemical))
            {
                var chemicals = await ChemicalsFilledQuery
                    .Where(c => c.GetType() == typeof(Chemical))
                    .Where(c => EF.Functions.ILike(c.Name, $"{nameOrCas}%"))
                    .ToListAsync();
                result.AddRange(chemicals);
            }

            if (assetItemTypes.Contains(AssetItemType.Consumable))
            {
                var consumables = await ConsumablesFilledQuery
                    .Where(c => EF.Functions.ILike(c.Name, $"{nameOrCas}%"))
                    .ToListAsync();
                result.AddRange(consumables);
            }

            if (assetItemTypes.Contains(AssetItemType.GasCylinder))
            {
                var gasCylinders = await GasCylindersFilledQuery
                    .Where(g => EF.Functions.ILike(g.Name, $"{nameOrCas}%"))
                    .ToListAsync();
                result.AddRange(gasCylinders);
            }

            if (assetItemTypes.Contains(AssetItemType.Solvent))
            {
                var solvents = await SolventsFilledQuery
                    .Where(s => EF.Functions.ILike(s.Name, $"{nameOrCas}%"))
                    .ToListAsync();
                result.AddRange(solvents);
            }
        }

        return result;
    }

    public async Task<List<AssetItem>> CombinedFilterAsync(
        Dictionary<string, string>? filters,
        List<AssetItemType> assetItemTypes
    )
    {
        // Start with an empty list to collect results
        List<AssetItem> result = new List<AssetItem>();

        // Check for each asset item type in the provided list and query them individually
        if (assetItemTypes.Contains(AssetItemType.Chemical))
        {
            var chemicals = ChemicalsFilledQuery.Where(item => item.GetType() == typeof(Chemical));
            result.AddRange(await ApplyFilters(chemicals, filters));
        }

        if (assetItemTypes.Contains(AssetItemType.Consumable))
        {
            result.AddRange(await ApplyFilters(ConsumablesFilledQuery, filters));
        }

        if (assetItemTypes.Contains(AssetItemType.GasCylinder))
        {
            result.AddRange(await ApplyFilters(GasCylindersFilledQuery, filters));
        }

        if (assetItemTypes.Contains(AssetItemType.Solvent))
        {
            result.AddRange(await ApplyFilters(SolventsFilledQuery, filters));
        }

        // Return the combined list of AssetItems
        return result;
    }

    private async Task<List<AssetItem>> ApplyFilters(
        IQueryable<AssetItem> query,
        Dictionary<string, string>? filters
    )
    {
        if (filters == null || filters.Count == 0)
            return await query.ToListAsync();

        foreach (var (key, value) in filters.Where(f => !string.IsNullOrWhiteSpace(f.Value)))
        {
            query = key switch
            {
                "Produkt" => ApplyProductFilter(query, value),
                "Artikelnummer" => query.Where(i => EF.Functions.ILike(i.ItemNumber, $"{value}%")),
                "Lieferant" => query.Where(i => EF.Functions.ILike(i.Shop, $"{value}%")),
                "Raum" => query.Where(i => EF.Functions.ILike(i.Room.Name, $"{value}%")),
                "Preis" => query.Where(i =>
                    EF.Functions.ILike(EF.Property<string>(i, "Price"), $"{value}%")
                ),
                _ => query,
            };
        }

        return await query.ToListAsync();
    }

    private static IQueryable<AssetItem> ApplyProductFilter(
        IQueryable<AssetItem> query,
        string value
    )
    {
        return value.All(c => char.IsDigit(c) || c == '-')
            ? query.Where(i => EF.Functions.ILike((i as Substance)!.Cas, $"{value}%"))
            : query.Where(i => EF.Functions.ILike(i.Name, $"{value}%"));
    }

    public async Task<List<AssetItem>> FilterAsync(Dictionary<string, string>? filters)
    {
        IQueryable<AssetItem> query = AssetItemsFilledQuery;
        foreach (var filter in filters)
        {
            switch (filter.Key)
            {
                case "Produkt":
                    if (string.IsNullOrWhiteSpace(filter.Value))
                        break;

                    if (filter.Value.All(c => char.IsDigit(c) || c == '-'))
                    {
                        query = SubstancesFilledQuery.Where(s =>
                            EF.Functions.ILike(s.Cas, $"{filter.Value}%")
                        );
                    }
                    else
                    {
                        query = query.Where(i => EF.Functions.ILike(i.Name, $"{filter.Value}%"));
                    }
                    break;

                case "Artikelnummer":
                    query = query.Where(i => EF.Functions.ILike(i.ItemNumber, $"{filter.Value}%"));
                    break;

                case "Lieferant":
                    query = query.Where(i => EF.Functions.ILike(i.Shop, $"{filter.Value}%"));
                    break;

                case "Raum":
                    query = query.Where(i => EF.Functions.ILike(i.Room.Name, $"{filter.Value}%"));
                    break;

                case "Preis":
                    query = query.Where(i =>
                        EF.Functions.ILike(
                            EF.Property<string>(i, "Price").ToString(),
                            $"{filter.Value}%"
                        )
                    );
                    break;

                default:
                    return await GetAllAsync();
            }
        }

        return await query.ToListAsync();
    }

    public async Task<List<AssetItem>> FilterTypeAsync(List<AssetItemType> assetItemTypes)
    {
        // Start with an empty list to collect results
        List<AssetItem> result = new List<AssetItem>();

        // Check for each asset item type in the provided list and query them individually
        if (assetItemTypes.Contains(AssetItemType.Chemical))
        {
            var chemicals = await ChemicalsFilledQuery
                .Where(item => item.GetType() == typeof(Chemical))
                .ToListAsync();
            result.AddRange(chemicals);
        }

        if (assetItemTypes.Contains(AssetItemType.Consumable))
        {
            var consumables = await ConsumablesFilledQuery.ToListAsync();
            result.AddRange(consumables);
        }

        if (assetItemTypes.Contains(AssetItemType.GasCylinder))
        {
            var gasCylinders = await GasCylindersFilledQuery.ToListAsync();
            result.AddRange(gasCylinders);
        }

        if (assetItemTypes.Contains(AssetItemType.Solvent))
        {
            var solvents = await SolventsFilledQuery.ToListAsync();
            result.AddRange(solvents);
        }

        // Return the combined list of AssetItems
        return result;
    }

    public async Task<ErrorOr<Created>> CreateAsync(CreateAssetItemDto createModel)
    {
        var room = await _context.Rooms.SingleOrDefaultAsync(r => r.Id == createModel.RoomId);
        if (room is null)
        {
            return Error.NotFound(
                "Assetitem.create",
                $"Given RoomId {createModel.RoomId} was not found."
            );
        }

        List<Hazard> hazards = [];
        if (createModel is CreateSubstanceDto createSubstanceDto)
        {
            foreach (var hazardId in createSubstanceDto.Hazards)
            {
                var hazard = await _context.Hazards.SingleOrDefaultAsync(h => h.Id != hazardId);
                if (hazard is null)
                {
                    return Error.NotFound(
                        "Assetitem.create",
                        $"Given HazardId {hazardId} was not found."
                    );
                }

                hazards.Add(hazard);
            }
        }

        AssetItem newItem = createModel switch
        {
            CreateSolventDto solvent => new Solvent
            {
                ItemNumber = createModel.ItemNumber,
                Name = createModel.Name,
                Note = createModel.Note,
                Price = createModel.Price,
                Shop = createModel.Shop,
                Room = room,
                Cas = solvent.Cas,
                Hazards = hazards,
                PriceUnit = solvent.PriceUnit,
                BindingSize = solvent.BindingSize,
                Purity = solvent.Purity,
            },
            CreateChemicalDto chemical => new Chemical
            {
                ItemNumber = createModel.ItemNumber,
                Name = createModel.Name,
                Note = createModel.Note,
                Price = createModel.Price,
                Shop = createModel.Shop,
                Room = room,
                Cas = chemical.Cas,
                Hazards = hazards,
                PriceUnit = chemical.PriceUnit,
                BindingSize = chemical.BindingSize,
                Purity = chemical.Purity,
            },
            CreateGasCylinderDto gas => new GasCylinder
            {
                ItemNumber = createModel.ItemNumber,
                Name = createModel.Name,
                Note = createModel.Note,
                Price = createModel.Price,
                Shop = createModel.Shop,
                Room = room,
                Cas = gas.Cas,
                Hazards = hazards,
                PriceUnit = gas.PriceUnit,
                Volume = gas.Volume,
                Pressure = gas.Pressure,
                Purity = gas.Purity,
            },
            CreateConsumableDto consumable => new Consumable
            {
                ItemNumber = createModel.ItemNumber,
                Name = createModel.Name,
                Note = createModel.Note,
                Price = createModel.Price,
                Shop = createModel.Shop,
                Room = room,
                Amount = consumable.Amount,
                Manufacturer = consumable.Manufacturer,
                SerialNumber = consumable.SerialNumber,
            },
            _ => throw new ArgumentException("Unsupported create model type", nameof(createModel)),
        };
        await _context.AssetItems.AddAsync(newItem);
        await _context.SaveChangesAsync();
        return Result.Created;
    }
}
