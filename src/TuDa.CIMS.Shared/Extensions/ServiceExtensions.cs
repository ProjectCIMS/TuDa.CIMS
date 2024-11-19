using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;

namespace TuDa.CIMS.Shared.Extensions;

public static class ServiceExtensions
{
    private record struct Service(
        Type ServiceType,
        ServiceRegistrationAttribute Attribute,
        Type ImplementationType
    );

    /// <summary>
    /// Registers all Services that have a derived attribute of <see cref="ServiceRegistrationAttribute"/>.
    /// </summary>
    /// <param name="services">ServiceCollection the services should be added.</param>
    /// <returns></returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        var types = Assembly.GetCallingAssembly().GetTypes();

        types
            .Where(t => t.GetCustomAttribute<ServiceRegistrationAttribute>() is not null)
            .Select(t => new Service
            {
                ServiceType = t,
                Attribute = t.GetCustomAttribute<ServiceRegistrationAttribute>()!,
                ImplementationType = types.First(i => i.IsAssignableTo(t) && t != i),
            })
            .ToList()
            .ForEach(services.AddService);

        return services;
    }

    private static void AddService(this IServiceCollection services, Service service)
    {
        switch (service)
        {
            case (var sType, ScopedServiceAttribute, var iType):
                services.AddScoped(sType, iType);
                break;
            case (var sType, SingletonServiceAttribute, var iType):
                services.AddSingleton(sType, iType);
                break;
            case (var sType, TransientServiceAttribute, var iType):
                services.AddTransient(sType, iType);
                break;
        }
    }

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
                        client.BaseAddress = new Uri(configuration["CIMS.Api"]!)
                    )
            );

        return services;
    }
}
