using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Repositories;

[ScopedService]
public class ConsumableTransactionRepository: IConsumableTransactionRepository
{
    private readonly CIMSDbContext _context;

    public ConsumableTransactionRepository(CIMSDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Returns all existing AssetItems of the database.
    /// </summary>
    public async Task<IEnumerable<ConsumableTransaction>> GetAllAsync()
    {
        return await _context.ConsumableTransactions.ToListAsync();
    }

    /// <summary>
    /// Returns an existing ConsumableTransaction with the specific id.
    /// </summary>
    /// <param name="id">the unique id of the ConsumableTransaction</param>
    public async Task<ConsumableTransaction?> GetOneAsync(Guid id)
    {
        return await _context
            .ConsumableTransactions.Where(i => i.Id == id)
            .SingleOrDefaultAsync();
    }

    /// <summary>
    /// Updates the amount of the specific Consumamble of the ConsumableTransaction and creates a new transaction for a consumable item.
    /// </summary>
    /// <param name="ConsumableTransactionDto"></param>

    public async Task<ErrorOr<Created>> CreateAsync(CreateConsumableTransactionDto consumableTransactionDto)
    {
        var consumable = await _context.Consumables
            .Where(i => i.Id == consumableTransactionDto.ConsumableId).SingleOrDefaultAsync();
        ;
        if (consumable is null)
        {
            return Error.NotFound("Consumable not found");
        }
        if (consumable.Amount + consumableTransactionDto.AmountChange < 0)
        {
            return Error.Failure("Negative amount of consumable is not possible.");
        }
        consumable.Amount =+ consumableTransactionDto.AmountChange;


        ConsumableTransaction consumableTransaction = new ConsumableTransaction()
        {
            Date = consumableTransactionDto.Date,
            Consumable = consumable,
            AmountChange = consumableTransactionDto.AmountChange,
            TransactionReason = consumableTransactionDto.TransactionReason,

        };
        _context.ConsumableTransactions.Add(consumableTransaction);
        await _context.SaveChangesAsync();
        return Result.Created;
    }


}
