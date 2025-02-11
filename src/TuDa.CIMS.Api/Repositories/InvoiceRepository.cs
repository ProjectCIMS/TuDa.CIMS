using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Repositories;

[ScopedService]
public class InvoiceRepository : IInvoiceRepository
{
    private readonly CIMSDbContext _context;

    public InvoiceRepository(CIMSDbContext context)
    {
        _context = context;
    }

    public Task<List<Purchase>> GetPurchasesInTimePeriod(
        Guid workingGroupId,
        DateTime beginDate,
        DateTime endDate
    ) =>
        _context
            .WorkingGroups.Where(wg => wg.Id == workingGroupId)
            .Include(wg => wg.Purchases)
            .SelectMany(wg => wg.Purchases)
            .Where(p =>
                (p.CompletionDate != null)
                && (p.CompletionDate.Value >= beginDate && p.CompletionDate.Value <= endDate)
            )
            .Include(p => p.Entries)
            .ThenInclude(e => e.AssetItem)
            .Include(p => p.Buyer)
            .ToListAsync();
}
