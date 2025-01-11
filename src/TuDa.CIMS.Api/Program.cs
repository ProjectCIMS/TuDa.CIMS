using QuestPDF;
using QuestPDF.Infrastructure;
using TuDa.CIMS.Api;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Api.Repositories;
using TuDa.CIMS.Api.Services;
using TuDa.CIMS.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<CIMSDbContext>("CIMS");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddServices();

builder.Services.AddControllers();

// Setup QuestPdf License
Settings.License = LicenseType.Community;
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();
var app = builder.Build();

app.MapDefaultEndpoints();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.SetupScalar();
}

app.UseHttpsRedirection();

await app.RunAsync();
