using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Repositories;

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
            .SelectMany(wg => wg.Purchases)
            .Where(p =>
                (p.Completed && p.CompletionDate != null)
                && (p.CompletionDate.Value >= beginDate && p.CompletionDate.Value <= endDate)
            )
            .ToListAsync();

    public Task<Professor> GetProfessorOfWorkingGroup(Guid workingGroupId) =>
        _context
            .WorkingGroups.Where(wg => wg.Id == workingGroupId)
            .Select(wg => wg.Professor)
            .SingleAsync();
}
