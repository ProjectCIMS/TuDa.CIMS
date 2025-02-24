using TuDa.CIMS.Shared.Models;

namespace TuDa.CIMS.Api.Interfaces;

public interface IConsumableService
{
    /// <summary>
    /// Retrieves the statistics of a specific consumable for a given year.
    /// </summary>
    /// <param name="consumableId">The unique identifier of the consumable.</param>
    /// <param name="year">The year for which the statistics are to be retrieved.</param>
    public Task<ErrorOr<ConsumableStatistics>> GetStatistics(Guid consumableId, int year);
}
