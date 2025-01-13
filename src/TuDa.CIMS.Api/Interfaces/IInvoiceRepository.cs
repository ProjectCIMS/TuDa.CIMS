using TuDa.CIMS.Api.Models;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IInvoiceRepository
{
    public Task<List<Purchase>> GetPurchasesInTimePeriod(
        Guid workingGroupId,
        DateTime beginDate,
        DateTime endDate
    );

    public Task<Professor?> GetProfessorOfWorkingGroup(Guid workingGroupId);
}
