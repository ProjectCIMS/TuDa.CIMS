using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Test.Integration;

public class CIMSApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _databaseContainer = new PostgreSqlBuilder()
        .WithUsername("workshop")
        .WithPassword("password")
        .WithDatabase("CIMS")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(x =>
        {
            x.Remove(x.Single(a => typeof(DbContextOptions<CIMSDbContext>) == a.ServiceType));
            x.Remove(x.Single(a => typeof(CIMSDbContext) == a.ServiceType));
            x.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Critical);
            });
            x.AddDbContext<CIMSDbContext>(
                a =>
                {
                    a.UseNpgsql(
                        _databaseContainer.GetConnectionString(),
                        options => options.EnableRetryOnFailure(0)
                    );
                },
                ServiceLifetime.Singleton
            );

            AssertionOptions.AssertEquivalencyUsing(options =>
                options.Excluding(member =>
                    member.DeclaringType.IsAssignableTo(typeof(BaseEntity)) &&
                    (member.Name == nameof(BaseEntity.CreatedAt) || member.Name == nameof(BaseEntity.UpdatedAt)))
            );
        });
    }

    public async Task InitializeAsync()
    {
        await _databaseContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _databaseContainer.StopAsync();
    }
}
