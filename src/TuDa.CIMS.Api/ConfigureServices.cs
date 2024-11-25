using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TuDa.CIMS.Api.Database;

namespace TuDa.CIMS.Api;

public static class ConfigureServices
{
    /// <summary>
    /// Configures the DbContext with the connection string.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CIMSDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("CIMS"));
        });
        return services;
    }

    /// <summary>
    /// Configures ScalarUi with a specified theme and sets C# HttpClient as the default client.
    /// </summary>
    public static WebApplication SetupScalar(this WebApplication app)
    {
        app.MapScalarApiReference(options =>
        {
            options.WithTitle("CIMS Api")
                .WithTheme(ScalarTheme.DeepSpace)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });

        return app;
    }
}
