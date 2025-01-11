using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Repositories;

public class ConsumableTransactionRepository: IConsumableTransactionRepository
{
    private readonly CIMSDbContext _context;

    public ConsumableTransactionRepository(CIMSDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Created>> RunAsync(CreateConsumableTransactionDto ConsumableTransactionDto)
    {
        var consumable = await _context.AssetItems.Include(i => i.Room).Where(i => i.Id == ConsumableTransactionDto.ConsumableId).SingleOrDefaultAsync();;

        Consumable castedConsumable = (Consumable)consumable;

        castedConsumable.Amount =+ ConsumableTransactionDto.AmountChange;


        await _context.SaveChangesAsync();
    }
}
