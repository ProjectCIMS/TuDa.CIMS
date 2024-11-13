using Scalar.AspNetCore;

namespace TuDa.CIMS.Api;

public static class ConfigureServices
{
    /// <summary>
    /// Sets up ScalarUi with a theme and Csharp HttpClient as default client.
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
