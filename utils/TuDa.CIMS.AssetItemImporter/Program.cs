using Cocona;
using Refit;
using TuDa.CIMS.AssetItemImporter;
using TuDa.CIMS.Web.Services;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddRefitClient<IAssetItemApi>();

var app = builder.Build();

app.AddCommand("check", Commands.CheckExcel);
app.AddCommand("import", Commands.ImportExcel);

await app.RunAsync();
