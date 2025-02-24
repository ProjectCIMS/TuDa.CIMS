using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TuDa.CIMS.Api.Database;

namespace TuDa.CIMS.Api.Test.Integration;

public class ControllerTestBase : IClassFixture<CIMSApiFactory>
{
    protected readonly HttpClient Client;
    protected readonly CIMSDbContext DbContext;

    public ControllerTestBase(CIMSApiFactory apiFactory)
    {
        Client = apiFactory.CreateClient();

        var scope = apiFactory.Services.CreateScope();
        DbContext = scope.ServiceProvider.GetRequiredService<CIMSDbContext>();

        DbContext.Database.EnsureDeleted();
        DbContext.Database.Migrate();
    }
}
