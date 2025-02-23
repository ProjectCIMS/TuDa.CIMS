var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("cims-postgres").WithLifetime(ContainerLifetime.Persistent);
var postgresDb = postgres.AddDatabase("CIMS");

var api = builder
    .AddProject<Projects.TuDa_CIMS_Api>("cims-api")
    .WithReference(postgresDb)
    .WaitFor(postgres);

builder.AddProject<Projects.TuDa_CIMS_Web>("cims-web").WithReference(api).WaitFor(api);

builder
    .AddProject<Projects.TuDa_CIMS_TestingDataService>("cims-testing-data")
    .WithReference(postgresDb)
    .WaitFor(api);

await builder.Build().RunAsync();
