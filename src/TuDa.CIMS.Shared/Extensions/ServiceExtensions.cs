using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;

namespace TuDa.CIMS.Shared.Extensions;

public static class ServiceExtensions
{
    /// <summary>
    /// Registers all RefitClients that have a <see cref="RefitClientAttribute"/>
    /// The base url must be defined in the appsettings.json under `CIMS.Api`
    /// </summary>
    /// <param name="services">ServiceCollection the services should be added.</param>
    /// <param name="configuration">The configuration with the base url defined.</param>
    public static IServiceCollection AddRefitClients(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        Assembly
            .GetCallingAssembly()
            .GetTypes()
            .Where(t => t.GetCustomAttribute<RefitClientAttribute>() is not null)
            .ToList()
            .ForEach(clientType =>
                services
                    .AddRefitClient(clientType)
                    .ConfigureHttpClient(client =>
                        client.BaseAddress = new Uri(
                            configuration["Api:BaseUrl"]!
                                + clientType.GetCustomAttribute<RefitClientAttribute>()?.BaseRoute
                        )
                    )
            );
        return services;
    }
}
