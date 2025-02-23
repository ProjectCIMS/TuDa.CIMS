using MudBlazor.Services;
using MudBlazor.Translations;
using TuDa.CIMS.Shared.Extensions;
using TuDa.CIMS.Web.Components;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRefitClients();

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddMudServices();
builder.Services.AddServices();
builder.Services.AddMudTranslations();

builder.Services.AddHealthChecks().AddCheck<ApiHealthCheck>("api");

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapHealthEndpoint();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

await app.RunAsync();
