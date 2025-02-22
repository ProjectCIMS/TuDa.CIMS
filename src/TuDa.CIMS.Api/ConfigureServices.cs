using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TuDa.CIMS.Api.Database;

namespace TuDa.CIMS.Api;

public static class ConfigureServices
{
    /// <summary>
    /// Configures ScalarUi with a specified theme and sets C# HttpClient as the default client.
    /// </summary>
    public static WebApplication SetupScalar(this WebApplication app)
    {
        app.MapScalarApiReference(options =>
        {
            //This is needed to clear the cashed IPs to work with Aspire.
            options.Servers = [];
            options
                .WithTitle("CIMS Api")
                .WithTheme(ScalarTheme.DeepSpace)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });

        return app;
    }

    public static IServiceCollection AddJsonDecoder(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        return services;
    }

    public static async Task<IApplicationBuilder> MigrateDatabase(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CIMSDbContext>();
        await context.Database.MigrateAsync();
        return builder;
    }
}
