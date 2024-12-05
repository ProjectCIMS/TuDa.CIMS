using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;

namespace TuDa.CIMS.Shared.Extensions;

public static class WebAppBuilderExtension
{
    /// <summary>
    /// Registers all RefitClients that have a <see cref="RefitClientAttribute"/>
    /// The base url must be defined in the appsettings.json under `CIMS:Api`
    /// </summary>
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
                            builder.Configuration["Api:BaseUrl"]!
                                + clientType.GetCustomAttribute<RefitClientAttribute>()?.BaseRoute
                        )
                    )
            );
        return builder;
    }
}
