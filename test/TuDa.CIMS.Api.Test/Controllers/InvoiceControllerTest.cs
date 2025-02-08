using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TuDa.CIMS.Api.Controllers;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Test.Integration;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Models;
using TuDa.CIMS.Shared.Test.Faker;

namespace TuDa.CIMS.Api.Test.Controllers;

[TestSubject(typeof(InvoiceController))]
public class InvoiceControllerTest : IClassFixture<CIMSApiFactory>
{
    private readonly HttpClient _client;
    private readonly CIMSDbContext _dbContext;

    private const double DoublePrecision = 0.001;

    public InvoiceControllerTest(CIMSApiFactory apiFactory)
    {
        _client = apiFactory.CreateClient();

        var scope = apiFactory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<CIMSDbContext>();

        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.Migrate();
    }

    [Fact]
    public async Task GetInvoiceStatistics_ShouldReturnCorrectStatistics_WhenDateRangeIsGiven()
    {
        WorkingGroup workingGroup = new WorkingGroupFaker(purchases: []);
        List<Purchase> purchases = new PurchaseFaker(workingGroup, completed: true).Generate(10);

        for (int i = 0; i < purchases.Count; i++)
        {
            purchases[i].CompletionDate = new DateOnly(2000, 1, 1 + i).ToDateTime(
                TimeOnly.MinValue,
                DateTimeKind.Utc
            );
        }

        InvoiceStatistics expected = PurchasesToInvoiceStatistics(purchases[3..8]);

        var startDate = new DateOnly(2000, 1, 4);
        var endDate = new DateOnly(2000, 1, 8);

        workingGroup.Purchases = purchases;

        await _dbContext.WorkingGroups.AddAsync(workingGroup);
        await _dbContext.SaveChangesAsync();

        var response = await _client.GetAsync(
            $"api/working-groups/{workingGroup.Id}/invoices/statistics?beginDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}"
        );

        response.IsSuccessStatusCode.Should().BeTrue();

        var result = await response.Content.ReadFromJsonAsync<InvoiceStatistics>();

        result.Should().NotBeNull();
        result
            .Should()
            .BeEquivalentTo(
                expected,
                options =>
                    options
                        .Using<double>(ctx =>
                            ctx.Subject.Should().BeApproximately(ctx.Expectation, DoublePrecision)
                        )
                        .WhenTypeIs<double>()
            );
    }

    [Fact]
    public async Task GetInvoiceStatistics_ShouldReturnCorrectStatistics_WhenMultiplePurchasesPresent()
    {
        WorkingGroup workingGroup = new WorkingGroupFaker(purchases: []);
        List<Purchase> purchases = new PurchaseFaker(workingGroup, completed: true).GenerateBetween(
            4,
            10
        );

        InvoiceStatistics expected = PurchasesToInvoiceStatistics(purchases);

        workingGroup.Purchases = purchases;

        await _dbContext.WorkingGroups.AddAsync(workingGroup);
        await _dbContext.SaveChangesAsync();

        var response = await _client.GetAsync(
            $"api/working-groups/{workingGroup.Id}/invoices/statistics"
        );

        response.IsSuccessStatusCode.Should().BeTrue();

        var result = await response.Content.ReadFromJsonAsync<InvoiceStatistics>();

        result.Should().NotBeNull();
        result
            .Should()
            .BeEquivalentTo(
                expected,
                options =>
                    options
                        .Using<double>(ctx =>
                            ctx.Subject.Should().BeApproximately(ctx.Expectation, DoublePrecision)
                        )
                        .WhenTypeIs<double>()
            );
    }

    [Fact]
    public async Task GetInvoiceStatistics_ShouldSkipPurchases_WhenPurchaseUncompleted()
    {
        WorkingGroup workingGroup = new WorkingGroupFaker(purchases: []);
        Purchase completed = new PurchaseFaker(workingGroup, completed: true);
        Purchase uncompleted = new PurchaseFaker(workingGroup, completed: false);

        InvoiceStatistics expected = PurchasesToInvoiceStatistics([completed]);

        workingGroup.Purchases = [completed, uncompleted];

        await _dbContext.WorkingGroups.AddAsync(workingGroup);
        await _dbContext.SaveChangesAsync();

        var response = await _client.GetAsync(
            $"api/working-groups/{workingGroup.Id}/invoices/statistics"
        );

        response.IsSuccessStatusCode.Should().BeTrue();

        var result = await response.Content.ReadFromJsonAsync<InvoiceStatistics>();

        result.Should().NotBeNull();
        result
            .Should()
            .BeEquivalentTo(
                expected,
                options =>
                    options
                        .Using<double>(ctx =>
                            ctx.Subject.Should().BeApproximately(ctx.Expectation, DoublePrecision)
                        )
                        .WhenTypeIs<double>()
            );
    }

    [Fact]
    public async Task GetInvoiceDocument_ShouldReturnAnPdfDocument()
    {
        WorkingGroup workingGroup = new WorkingGroupFaker(purchases: []);
        List<Purchase> purchases = new PurchaseFaker(workingGroup, completed: true).GenerateBetween(
            4,
            10
        );
        var information = new AdditionalInvoiceInformation { InvoiceNumber = "Number" };

        workingGroup.Purchases = purchases;

        await _dbContext.WorkingGroups.AddAsync(workingGroup);
        await _dbContext.SaveChangesAsync();

        var response = await _client.PostAsync(
            $"api/working-groups/{workingGroup.Id}/invoices/document",
            JsonContent.Create(information)
        );

        response.IsSuccessStatusCode.Should().BeTrue();
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/pdf");

        byte[] result = await response.Content.ReadAsByteArrayAsync();

        // Convert the first 5 bytes to a string
        string pdfHeader = System.Text.Encoding.ASCII.GetString(result, 0, 5);

        pdfHeader.Should().Be("%PDF-");
    }

    private static InvoiceStatistics PurchasesToInvoiceStatistics(List<Purchase> purchases) =>
        new()
        {
            TotalPriceChemicals = purchases
                .SelectMany(p => p.Entries)
                .Where(e => e.AssetItem is Chemical and not Solvent)
                .Aggregate(0.0, (i, entry) => i + entry.TotalPrice),
            TotalPriceConsumables = purchases
                .SelectMany(p => p.Entries)
                .Where(e => e.AssetItem is Consumable)
                .Aggregate(0.0, (i, entry) => i + entry.TotalPrice),
            TotalPriceSolvents = purchases
                .SelectMany(p => p.Entries)
                .Where(e => e.AssetItem is Solvent)
                .Aggregate(0.0, (i, entry) => i + entry.TotalPrice),
            TotalPriceGasCylinders = purchases
                .SelectMany(p => p.Entries)
                .Where(e => e.AssetItem is GasCylinder)
                .Aggregate(0.0, (i, entry) => i + entry.TotalPrice),
        };
}
