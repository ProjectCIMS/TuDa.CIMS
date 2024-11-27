using TuDa.CIMS.Api.Database;
using TuDa.CIMS.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder
    .Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddNpgsqlDbContext<CIMSDbContext>("CIMS");

var host = builder.Build();
await host.RunAsync();
