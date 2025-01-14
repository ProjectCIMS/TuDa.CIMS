using Refit;

namespace TuDa.CIMS.Web.Extensions;

public static class ApiResponseExtension
{
    private static ErrorOr<TResult> ToErrorOr<TResult>(this IApiResponse<TResult> apiResponse)
    {
        return apiResponse.IsSuccessful switch
        {
            true => apiResponse.Content,
            false => apiResponse.Error.ToError(),
        };
    }

    private static ErrorOr<TResult> ToErrorOr<TResult>(this IApiResponse apiResponse)
        where TResult : new()
    {
        return apiResponse.IsSuccessful switch
        {
            true => new TResult(),
            false => apiResponse.Error.ToError(),
        };
    }

    public static async Task<ErrorOr<TResult>> ToErrorOrAsync<TResult>(
        this Task<IApiResponse<TResult>> apiResponseTask
    )
    {
        return (await apiResponseTask).ToErrorOr();
    }

    public static async Task<ErrorOr<TResult>> ToErrorOrAsync<TResult>(
        this Task<IApiResponse> apiResponseTask
    )
        where TResult : new()
    {
        return (await apiResponseTask).ToErrorOr<TResult>();
    }

    private static Error ToError(this ApiException exception)
    {
        return Error.Custom(
            (int)exception.StatusCode,
            exception.ReasonPhrase ?? string.Empty,
            exception.Message,
            new Dictionary<string, object>
            {
                { "url", exception.Uri?.ToString() ?? "None" },
                { "method", exception.HttpMethod },
                { "data", exception.Data },
                { "problemDetails", exception.Content ?? string.Empty },
            }
        );
    }
}
