using Cocona;
using TuDa.CIMS.AssetItemImporter;

var builder = CoconaApp.CreateBuilder();

var app = builder.Build();

app.AddCommand("check", Commands.CheckExcel);

await app.RunAsync();
