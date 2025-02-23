using System.Reflection;
using Refit;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;

namespace TuDa.CIMS.Shared.Extensions;

public static class WebAppBuilderExtension
{
    /// <summary>
    /// Registers all Refit clients annotated with the <see cref="RefitClientAttribute"/>.
    ///
    /// <p>
    ///     In development:
    /// </p>
    /// <ul>
    ///     <li>
    ///         If Aspire is used to start the project, 'Api:Aspire:ServiceName' need to be set.
    ///         The base URL is derived from that service name
    ///         and 'Api:Aspire:https' determines the protocol (HTTP/HTTPS).
    ///     </li>
    ///     <li>
    ///         Otherwise, 'Api:BaseUrl' is used.
    ///     </li>
    /// </ul>
    ///
    /// In other environments, 'Api:BaseUrl' is required.
    /// </summary>
    /// <exception cref="ArgumentException">If the necessary configuration keys are not present.</exception>
    public static TBuilder AddRefitClients<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        Assembly
            .GetCallingAssembly()
            .GetTypes()
            .Where(t => t.GetCustomAttribute<RefitClientAttribute>() is not null)
            .ToList()
            .ForEach(clientType =>
                builder
                    .Services.AddRefitClient(clientType)
                    .ConfigureHttpClient(client =>
                        client.BaseAddress = new Uri(
                            builder.GetApiBaseUrl()
                                + clientType.GetCustomAttribute<RefitClientAttribute>()?.BaseRoute
                        )
                    )
            );
        return builder;
    }

    private static string GetApiBaseUrl<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        if (!builder.Environment.IsDevelopment())
        {
            return builder.Configuration["Api:BaseUrl"]
                ?? throw new ArgumentException("Api:BaseUrl is not set");
        }

        string apiServiceName =
            builder.Configuration["Api:Aspire:ServiceName"]
            ?? throw new ArgumentException("Api:ServiceName is not set");

        return builder.Configuration.Properties.TryGetValue(
            $"services:{apiServiceName}",
            out _
        ) switch
        {
            false => builder.Configuration["Api:BaseUrl"]
                ?? throw new ArgumentException("Api:BaseUrl is not set"),
            true => (builder.Configuration.GetValue<bool?>("Api:Aspire:https") ?? false) switch
            {
                true => $"https://{apiServiceName}",
                false => $"http://{apiServiceName}",
            },
        };
    }
}
