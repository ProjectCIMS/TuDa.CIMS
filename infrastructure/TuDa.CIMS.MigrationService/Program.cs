using TuDa.CIMS.Api;
using TuDa.CIMS.MigrationService;

var builder = Host.CreateApplicationBuilder(args);
builder.AddNpgsqlDbContext<ApplicationDbContext>("CIMS");
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();
