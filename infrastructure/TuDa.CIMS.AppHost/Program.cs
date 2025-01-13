var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("cims-postgres").WithLifetime(ContainerLifetime.Persistent);
var postgresDb = postgres.AddDatabase("CIMS");

var migration = builder
    .AddProject<Projects.TuDa_CIMS_MigrationService>("cims-migration")
    .WithReference(postgresDb)
    .WaitFor(postgres);

var api = builder
    .AddProject<Projects.TuDa_CIMS_Api>("cims-api")
    .WithReference(postgresDb).
    WaitForCompletion(migration);

builder.AddProject<Projects.TuDa_CIMS_Web>("cims-web").WithReference(api).WaitFor(api);

await builder.Build().RunAsync();
