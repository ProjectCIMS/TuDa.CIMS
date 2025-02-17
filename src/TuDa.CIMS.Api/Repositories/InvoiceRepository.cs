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
                (p.CompletionDate != null) // Purchase is not completed
                && (p.CompletionDate.Value >= beginDate && p.CompletionDate.Value <= endDate) // Purchase is in DateSpan
                && (p.Successor == null) // Purchase is not invalidated
            )
            .Include(p => p.Entries)
            .ThenInclude(e => e.AssetItem)
            .Include(p => p.Buyer)
            .ToListAsync();
}
