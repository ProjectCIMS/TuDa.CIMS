using Refit;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Models;
using TuDa.CIMS.Web.Extensions;

namespace TuDa.CIMS.Web.Services;

/// <summary>
/// Refit client interface for performing operations on Invoices.
/// </summary>
[RefitClient("/api/invoices")]
public interface IInvoiceApi
{
    /// <summary>
    /// Asynchronously retrieves invoice statistics for a specified working group and optional date range.
    /// </summary>
    /// <param name="workingGroupId">The unique identifier of the working group.</param>
    /// <param name="beginDate">The optional start date of the period to retrieve statistics for.</param>
    /// <param name="endDate">The optional end date of the period to retrieve statistics for.</param>
    public async Task<ErrorOr<InvoiceStatistics>> GetStatisticsAsync(
        Guid workingGroupId,
        DateOnly? beginDate = null,
        DateOnly? endDate = null
    ) => await GetStatisticsAsyncInternal(workingGroupId, beginDate, endDate).ToErrorOrAsync();

    /// <summary>
    /// Internal method to asynchronously retrieve invoice statistics for a specified working group and optional date range.
    /// </summary>
    /// <param name="workingGroupId">The unique identifier of the working group.</param>
    /// <param name="beginDate">The optional start date of the period to retrieve statistics for.</param>
    /// <param name="endDate">The optional end date of the period to retrieve statistics for.</param>
    [Get("/{workingGroupId}/statistics")]
    protected Task<IApiResponse<InvoiceStatistics>> GetStatisticsAsyncInternal(
        Guid workingGroupId,
        [Query] DateOnly? beginDate,
        [Query] DateOnly? endDate
    );

    /// <summary>
    /// Asynchronously retrieves a PDF document for a specified working group and optional date range,
    /// including additional invoice information.
    /// </summary>
    /// <param name="workingGroupId">The unique identifier of the working group.</param>
    /// <param name="additionalInformation">The additional invoice information to include in the PDF.</param>
    /// <param name="beginDate">The optional start date of the period to retrieve the PDF for.</param>
    /// <param name="endDate">The optional end date of the period to retrieve the PDF for.</param>
    public async Task<ErrorOr<byte[]>> GetPdfAsync(
        Guid workingGroupId,
        AdditionalInvoiceInformation additionalInformation,
        DateOnly? beginDate = null,
        DateOnly? endDate = null
    ) =>
        await GetPdfAsyncInternal(workingGroupId, additionalInformation, beginDate, endDate)
            .ToErrorOrAsync()
            .ThenAsync(content => content.ReadAsByteArrayAsync());

    /// <summary>
    /// Internal method to asynchronously retrieve a PDF document for a specified working group
    /// and optional date range, including additional invoice information.
    /// </summary>
    /// <param name="workingGroupId">The unique identifier of the working group.</param>
    /// <param name="additionalInformation">The additional invoice information to include in the PDF.</param>
    /// <param name="beginDate">The optional start date of the period to retrieve the PDF for.</param>
    /// <param name="endDate">The optional end date of the period to retrieve the PDF for.</param>
    [Post("/{workingGroupId}/document")]
    protected Task<IApiResponse<HttpContent>> GetPdfAsyncInternal(
        Guid workingGroupId,
        [Body] AdditionalInvoiceInformation additionalInformation,
        [Query] DateOnly? beginDate,
        [Query] DateOnly? endDate
    );
}
