using Refit;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Models;
using TuDa.CIMS.Web.Extensions;

namespace TuDa.CIMS.Web.Services;

[RefitClient("/api/consumables")]
public interface IConsumableApi
{
    /// <summary>
    /// Asynchronously retrieves consumable statistics for a specified consumable and year.
    /// </summary>
    /// <param name="consumableId">The unique identifier of the consumable.</param>
    /// <param name="year">The year to retrieve the information for.</param>
    public async Task<ErrorOr<ConsumableStatistics>> GetStatisticsAsync(
        Guid consumableId,
        int year
    ) => await GetStatisticsAsyncInternal(consumableId, year).ToErrorOrAsync();

    /// <summary>
    /// Internal method to asynchronously retrieve consumable statistics for a specified consumable and year.
    /// </summary>
    /// <param name="consumableId">The unique identifier of the consumable.</param>
    /// <param name="year">The year to retrieve the information for.</param>
    [Get($"/{{{nameof(consumableId)}}}/statistics/{{{nameof(year)}}}")]
    protected Task<IApiResponse<ConsumableStatistics>> GetStatisticsAsyncInternal(
        Guid consumableId,
        int year
    );
}
