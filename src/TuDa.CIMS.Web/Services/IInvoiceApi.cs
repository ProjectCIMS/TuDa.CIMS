using Microsoft.JSInterop;
using Refit;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Models;
using TuDa.CIMS.Web.Extensions;

namespace TuDa.CIMS.Web.Services;

/// <summary>
/// Refit client interface for performing operations on Invoices.
/// </summary>
[RefitClient("/api/working-groups")]
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
    [Get("/{workingGroupId}/invoices/statistics")]
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
    /// Asynchronously opens a PDF document in the browser for a specified working group and optional date range,
    /// including additional invoice information, and triggers a download in the browser.
    /// </summary>
    /// <param name="workingGroupId">The unique identifier of the working group.</param>
    /// <param name="additionalInformation">The additional invoice information to include in the PDF.</param>
    /// <param name="jsRuntime">The JavaScript runtime to use for invoking browser download functionality.</param>
    /// <param name="beginDate">The optional start date of the period to retrieve the PDF for.</param>
    /// <param name="endDate">The optional end date of the period to retrieve the PDF for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an ErrorOr object with the success status.</returns>
    public async Task<ErrorOr<Success>> OpenPdfAsync(
        Guid workingGroupId,
        AdditionalInvoiceInformation additionalInformation,
        IJSRuntime jsRuntime,
        DateOnly? beginDate = null,
        DateOnly? endDate = null
    ) =>
        await GetPdfAsyncInternal(workingGroupId, additionalInformation, beginDate, endDate)
            .ToErrorOrAsync()
            .ThenAsync<HttpContent, Success>(
                async (content) =>
                {
                    try
                    {
                        using var streamRef = new DotNetStreamReference(
                            await content.ReadAsStreamAsync()
                        );

                        await jsRuntime.InvokeVoidAsync(
                            "downloadFileFromStream",
                            $"Rechnung-{additionalInformation.InvoiceNumber}.pdf",
                            streamRef
                        );
                        return Result.Success;
                    }
                    catch (Exception e)
                    {
                        return Error.Failure(e.GetType().Name, e.Message);
                    }
                }
            );

    /// <summary>
    /// Internal method to asynchronously retrieve a PDF document for a specified working group
    /// and optional date range, including additional invoice information.
    /// </summary>
    /// <param name="workingGroupId">The unique identifier of the working group.</param>
    /// <param name="additionalInformation">The additional invoice information to include in the PDF.</param>
    /// <param name="beginDate">The optional start date of the period to retrieve the PDF for.</param>
    /// <param name="endDate">The optional end date of the period to retrieve the PDF for.</param>
    [Post("/{workingGroupId}/invoices/document")]
    protected Task<IApiResponse<HttpContent>> GetPdfAsyncInternal(
        Guid workingGroupId,
        [Body] AdditionalInvoiceInformation additionalInformation,
        [Query] DateOnly? beginDate,
        [Query] DateOnly? endDate
    );
}
