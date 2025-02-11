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
builder.Services.AddSwaggerGen(options =>
{
    options.UseAllOfForInheritance();
    options.UseOneOfForPolymorphism();
});

builder.Services.AddServices();
builder.Services.AddProblemDetails();
builder.Services.AddJsonDecoder();

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
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.SetupScalar();
}

app.UseHttpsRedirection();

await app.RunAsync();
