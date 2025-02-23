using Microsoft.Extensions.Diagnostics.HealthChecks;

public class ApiHealthCheck : IHealthCheck
{
    private readonly string _apiUrl;
    private readonly HttpClient _httpClient;
    private const string HEALTH_ENDPOINT = "health";

    public ApiHealthCheck(IConfigurationManager configuration)
    {
        _apiUrl =
            configuration["Api:BaseUrl"]
            ?? throw new ArgumentNullException(
                "ApiSettings:BaseUrl",
                "API base URL must be configured"
            );

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_apiUrl),
            Timeout = TimeSpan.FromSeconds(30),
        };
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var response = await _httpClient.GetAsync(HEALTH_ENDPOINT, cancellationToken);

            // Read the response content for more detailed reporting
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy(
                    description: "API is healthy",
                    data: new Dictionary<string, object>
                    {
                        { "StatusCode", (int)response.StatusCode },
                        { "Content", content },
                        { "Endpoint", $"{_apiUrl}/{HEALTH_ENDPOINT}" },
                    }
                );
            }

            return HealthCheckResult.Unhealthy(
                description: "API health check failed",
                data: new Dictionary<string, object>
                {
                    { "StatusCode", (int)response.StatusCode },
                    { "Content", content },
                    { "Endpoint", $"{_apiUrl}/{HEALTH_ENDPOINT}" },
                }
            );
        }
        catch (HttpRequestException ex)
        {
            return HealthCheckResult.Unhealthy(
                description: "API is unreachable",
                exception: ex,
                data: new Dictionary<string, object>
                {
                    { "Endpoint", $"{_apiUrl}/{HEALTH_ENDPOINT}" },
                    { "Error", ex.Message },
                }
            );
        }
        catch (TaskCanceledException)
        {
            return HealthCheckResult.Unhealthy(
                description: "API health check timed out",
                data: new Dictionary<string, object>
                {
                    { "Endpoint", $"{_apiUrl}/{HEALTH_ENDPOINT}" },
                    { "Timeout", _httpClient.Timeout.TotalSeconds },
                }
            );
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                description: "An unexpected error occurred during health check",
                exception: ex,
                data: new Dictionary<string, object>
                {
                    { "Endpoint", $"{_apiUrl}/{HEALTH_ENDPOINT}" },
                    { "Error", ex.Message },
                }
            );
        }
    }

    // Implement IDisposable to properly clean up the HttpClient
    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
