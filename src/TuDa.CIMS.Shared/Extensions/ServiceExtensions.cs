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
        Type ImplementationType,
        ServiceRegistrationAttribute Attribute
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
            .Select(t => CreateService(t, types))
            .Distinct()
            .ToList()
            .ForEach(services.AddService);

        return services;
    }

    private static Service CreateService(Type t, Type[] types)
    {
        var attr = t.GetCustomAttribute<ServiceRegistrationAttribute>()!;
        if (t.IsInterface)
        {
            return new Service
            {
                ServiceType = attr.ServiceType ?? t,
                ImplementationType =
                    attr.ImplementationType ?? types.First(i => i.IsAssignableTo(t) && t != i),
                Attribute = attr,
            };
        }

        if (t.IsClass)
        {
            return new Service
            {
                ImplementationType = attr.ImplementationType ?? t,
                ServiceType =
                    attr.ServiceType
                    ?? (
                        t.GetInterfaces().FirstOrDefault()
                        ?? throw new ArgumentException(
                            "Classes need to be derived from another type to be registered."
                        )
                    ),
                Attribute = attr,
            };
        }

        throw new ArgumentException(
            $"A service registration is used on an unsupported type: {t.FullName}. Only interfaces and classes are supported."
        );
    }

    private static void AddService(this IServiceCollection services, Service service)
    {
        switch (service)
        {
            case (var sType, var iType, ScopedServiceAttribute attribute):
                services.AddKeyedScoped(sType, attribute.ServiceKey, iType);
                break;
            case (var sType, var iType, SingletonServiceAttribute attribute):
                services.AddKeyedSingleton(sType, attribute.ServiceKey, iType);
                break;
            case (var sType, var iType, TransientServiceAttribute attribute):
                services.AddKeyedTransient(sType, attribute.ServiceKey, iType);
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

    public static IServiceCollection AddDefaultLocalization(this IServiceCollection services) =>
        services.Configure<RequestLocalizationOptions>(options =>
        {
            string[] supportedCultures = ["de-DE"];
            options
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
        });
}
