using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Factories;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Constants;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Shared.Params;

namespace TuDa.CIMS.Api.Repositories;

[ScopedService]
public class AssetItemRepository : IAssetItemRepository
{
    private readonly CIMSDbContext _context;
    private readonly IConsumableTransactionRepository _consumableTransactionRepository;

    public AssetItemRepository(
        CIMSDbContext context,
        IConsumableTransactionRepository consumableTransactionRepository
    )
    {
        _context = context;
        _consumableTransactionRepository = consumableTransactionRepository;
    }

    /// <summary>
    /// Query for all <see cref="AssetItem"/>s from the DB with included properties.
    /// </summary>
    private IQueryable<AssetItem> AssetItemsQuery => _context.AssetItems;

    /// <summary>
    /// Query for all <see cref="Substance"/>s from the DB with included properties.
    /// </summary>
    private IQueryable<Substance> SubstancesQuery =>
        AssetItemsQuery.OfType<Substance>().Include(s => s.Hazards);

    /// <summary>
    /// Query for all <see cref="GasCylinder"/>s from the DB with included properties.
    /// </summary>
    private IQueryable<GasCylinder> GasCylindersQuery => SubstancesQuery.OfType<GasCylinder>();

    /// <summary>
    /// Query for all <see cref="Solvent"/>s from the DB with included properties.
    /// </summary>
    private IQueryable<Solvent> SolventsQuery => SubstancesQuery.OfType<Solvent>();

    /// <summary>
    /// Query for all <see cref="Chemical"/>s from the DB with included properties.
    /// </summary>
    private IQueryable<Chemical> ChemicalsQuery => SubstancesQuery.OfType<Chemical>();

    /// <summary>
    /// Query for all <see cref="Consumable"/>s from the DB with included properties.
    /// </summary>
    private IQueryable<Consumable> ConsumablesQuery => AssetItemsQuery.OfType<Consumable>();

    /// <summary>
    /// Returns all existing AssetItems of the database.
    /// </summary>
    public Task<List<AssetItem>> GetAllAsync() => AssetItemsQuery.ToListAsync();

    /// <summary>
    /// Returns an existing AssetItem with the specific id.
    /// </summary>
    /// <param name="id">the unique id of the AssetItem</param>
    public Task<AssetItem?> GetOneAsync(Guid id) =>
        AssetItemsQuery.SingleOrDefaultAsync(item => item.Id == id);

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
        existingItem.Room = updateModel.Room ?? existingItem.Room;

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
                consumable.Manufacturer = update.Manufacturer ?? consumable.Manufacturer;
                consumable.SerialNumber = update.SerialNumber ?? consumable.SerialNumber;
                break;

            default:
                return Error.Failure(
                    "Assetitem.update",
                    $"The provided update model does not match the type of the AssetItem with ID {id}."
                );
        }

        if (
            existingItem is Consumable con
            && updateModel is UpdateConsumableDto updateConsumable
            && updateConsumable.StockUpdate is not null
        )
        {
            var previousAmount = con.Amount;
            CreateConsumableTransactionDto createConsumableTransaction =
                new()
                {
                    ConsumableId = con.Id,
                    Date = DateTime.UtcNow,
                    AmountChange = updateConsumable.StockUpdate.Amount - previousAmount,
                    TransactionReason = updateConsumable.StockUpdate.Reason,
                };
            //amount of conusmable is now set in CreateAsync of ConsumableTransaction
            await _consumableTransactionRepository.CreateAsync(createConsumableTransaction);
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
            AssetItemsQuery,
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
            ? SubstancesQuery.Where(s => EF.Functions.ILike(s.Cas, $"{nameOrCas}%"))
            : AssetItemsQuery.Where(i => EF.Functions.ILike(i.Name, $"{nameOrCas}%"));
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
                var chemicals = await ChemicalsQuery
                    .Where(c => c.GetType() == typeof(Chemical))
                    .Where(c => EF.Functions.ILike(c.Cas, $"{nameOrCas}%"))
                    .ToListAsync();
                result.AddRange(chemicals);
            }

            if (assetItemTypes.Contains(AssetItemType.Solvent))
            {
                var solvents = await SolventsQuery
                    .Where(s => EF.Functions.ILike(s.Cas, $"{nameOrCas}%"))
                    .ToListAsync();
                result.AddRange(solvents);
            }
        }
        else
        {
            if (assetItemTypes.Contains(AssetItemType.Chemical))
            {
                var chemicals = await ChemicalsQuery
                    .Where(c => c.GetType() == typeof(Chemical))
                    .Where(c => EF.Functions.ILike(c.Name, $"{nameOrCas}%"))
                    .ToListAsync();
                result.AddRange(chemicals);
            }

            if (assetItemTypes.Contains(AssetItemType.Consumable))
            {
                var consumables = await ConsumablesQuery
                    .Where(c => EF.Functions.ILike(c.Name, $"{nameOrCas}%"))
                    .ToListAsync();
                result.AddRange(consumables);
            }

            if (assetItemTypes.Contains(AssetItemType.GasCylinder))
            {
                var gasCylinders = await GasCylindersQuery
                    .Where(g => EF.Functions.ILike(g.Name, $"{nameOrCas}%"))
                    .ToListAsync();
                result.AddRange(gasCylinders);
            }

            if (assetItemTypes.Contains(AssetItemType.Solvent))
            {
                var solvents = await SolventsQuery
                    .Where(s => EF.Functions.ILike(s.Name, $"{nameOrCas}%"))
                    .ToListAsync();
                result.AddRange(solvents);
            }
        }

        return result;
    }

    public async Task<List<AssetItem>> CombinedFilterAsync(AssetItemFilterDto filter)
    {
        // Start with an empty list to collect results
        List<AssetItem> result = new List<AssetItem>();

        // Check for each asset item type in the provided list and query them individually
        if (filter.AssetItemTypes!.Contains(AssetItemType.Chemical))
        {
            var chemicals = ChemicalsQuery.Where(item => item.GetType() == typeof(Chemical));
            result.AddRange(await ApplyFilters(chemicals, filter));
        }

        if (filter.AssetItemTypes.Contains(AssetItemType.Consumable))
        {
            result.AddRange(await ApplyFilters(ConsumablesQuery, filter));
        }

        if (filter.AssetItemTypes.Contains(AssetItemType.GasCylinder))
        {
            result.AddRange(await ApplyFilters(GasCylindersQuery, filter));
        }

        if (filter.AssetItemTypes.Contains(AssetItemType.Solvent))
        {
            result.AddRange(await ApplyFilters(SolventsQuery, filter));
        }

        // Return the combined list of AssetItems
        return result;
    }

    private static async Task<List<AssetItem>> ApplyFilters(
        IQueryable<AssetItem> query,
        AssetItemFilterDto filter
    )
    {
        if (
            string.IsNullOrEmpty(filter.Product)
            && string.IsNullOrEmpty(filter.Shop)
            && string.IsNullOrEmpty(filter.ItemNumber)
            && string.IsNullOrEmpty(filter.RoomName)
            && string.IsNullOrEmpty(filter.Price)
        )
            return await query.ToListAsync();

        if (!string.IsNullOrEmpty(filter.Product))
        {
            query = ApplyProductFilter(query, filter.Product);
        }

        if (!string.IsNullOrEmpty(filter.Shop))
        {
            query = query.Where(i => EF.Functions.ILike(i.Shop, $"{filter.Shop}%"));
        }

        if (!string.IsNullOrEmpty(filter.ItemNumber))
        {
            query = query.Where(i => EF.Functions.ILike(i.ItemNumber, $"{filter.ItemNumber}%"));
        }

        if (!string.IsNullOrEmpty(filter.RoomName))
        {
            query = query.Where(i => EF.Functions.ILike(i.Room.ToString(), $"{filter.RoomName}%"));
        }

        if (!string.IsNullOrEmpty(filter.Price))
        {
            query = query.Where(i =>
                EF.Functions.ILike(EF.Property<string>(i, "Price").ToString(), $"{filter.Price}%")
            );
        }
        return await query.ToListAsync();
    }

    private static IQueryable<AssetItem> ApplyProductFilter(
        IQueryable<AssetItem> query,
        string? value
    )
    {
        if (string.IsNullOrEmpty(value))
            return query;

        return value.All(c => char.IsDigit(c) || c == '-')
            ? query.Where(i => EF.Functions.ILike((i as Substance)!.Cas, $"{value}%"))
            : query.Where(i => EF.Functions.ILike(i.Name, $"{value}%"));
    }

    public async Task<List<AssetItem>> FilterAsync(AssetItemFilterDto filter)
    {
        IQueryable<AssetItem> query = AssetItemsQuery;
        foreach (var column in filter.AssetItemColumnsList)
        {
            switch (column)
            {
                case AssetItemColumns.Product:
                    if (string.IsNullOrWhiteSpace(filter.Product))
                        break;

                    if (filter.Product.All(c => char.IsDigit(c) || c == '-'))
                    {
                        query = SubstancesQuery.Where(s =>
                            EF.Functions.ILike(s.Cas, $"{filter.Product}%")
                        );
                    }
                    else
                    {
                        query = query.Where(i => EF.Functions.ILike(i.Name, $"{filter.Product}%"));
                    }
                    break;

                case "Artikelnummer":
                    query = query.Where(i =>
                        EF.Functions.ILike(i.ItemNumber, $"{filter.ItemNumber}%")
                    );
                    break;

                case "Lieferant":
                    query = query.Where(i => EF.Functions.ILike(i.Shop, $"{filter.Shop}%"));
                    break;

                case "Raum":
                    query = query.Where(i =>
                        EF.Functions.ILike(i.Room.ToString(), $"{filter.RoomName}%")
                    );
                    break;

                case "Preis":
                    query = query.Where(i =>
                        EF.Functions.ILike(
                            EF.Property<string>(i, "Price").ToString(),
                            $"{filter.Price}%"
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
            var chemicals = await ChemicalsQuery
                .Where(item => item.GetType() == typeof(Chemical))
                .ToListAsync();
            result.AddRange(chemicals);
        }

        if (assetItemTypes.Contains(AssetItemType.Consumable))
        {
            var consumables = await ConsumablesQuery.ToListAsync();
            result.AddRange(consumables);
        }

        if (assetItemTypes.Contains(AssetItemType.GasCylinder))
        {
            var gasCylinders = await GasCylindersQuery.ToListAsync();
            result.AddRange(gasCylinders);
        }

        if (assetItemTypes.Contains(AssetItemType.Solvent))
        {
            var solvents = await SolventsQuery.ToListAsync();
            result.AddRange(solvents);
        }

        // Return the combined list of AssetItems
        return result;
    }

    public async Task<ErrorOr<Created>> CreateAsync(CreateAssetItemDto createModel)
    {
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
                Room = createModel.Room,
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
                Room = createModel.Room,
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
                Room = createModel.Room,
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
                Room = createModel.Room,
                Amount = consumable.Amount,
                Manufacturer = consumable.Manufacturer,
                SerialNumber = consumable.SerialNumber,
            },
            _ => throw new ArgumentException("Unsupported create model type", nameof(createModel)),
        };

        if (newItem is Consumable con && createModel is CreateConsumableDto createConsumableModel)
        {
            ConsumableTransaction consumableTransaction =
                new()
                {
                    Consumable = con,
                    Date = DateTime.UtcNow,
                    AmountChange = con.Amount,
                    TransactionReason = createConsumableModel.ExcludeFromConsumableStatistics
                        ? TransactionReasons.Init
                        : TransactionReasons.Restock,
                };
            await _context.ConsumableTransactions.AddAsync(consumableTransaction);
        }
        await _context.AssetItems.AddAsync(newItem);
        await _context.SaveChangesAsync();
        return Result.Created;
    }
}
