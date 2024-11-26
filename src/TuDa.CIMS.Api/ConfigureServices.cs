using Scalar.AspNetCore;

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
            options
                .WithTitle("CIMS Api")
                .WithTheme(ScalarTheme.DeepSpace)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });

        return app;
    }
}
