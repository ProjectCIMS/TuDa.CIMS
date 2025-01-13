using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Test.Integration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Shared.Test.Faker;

namespace TuDa.CIMS.Api.Test.Controllers;

public class ConsumableTransactionTest : IClassFixture<CIMSApiFactory>
{
    private readonly HttpClient _client;
    private readonly CIMSDbContext _dbContext;

    public ConsumableTransactionTest(CIMSApiFactory apiFactory)
    {
        _client = apiFactory.CreateClient();

        var scope = apiFactory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<CIMSDbContext>();

        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.Migrate();
    }

    [Fact]
    public async Task GetAsync_ShouldReturnConsumableTransaction_WhenConsumableTransactionPresent()
    {
        // Arrange
        List<ConsumableTransaction> consumableTransactions =
        [
            new ConsumableTransactionFaker(),
            new ConsumableTransactionFaker(),
        ];

        await _dbContext.ConsumableTransactions.AddRangeAsync(consumableTransactions);
        await _dbContext.SaveChangesAsync();

        foreach (var consumableTransaction in consumableTransactions)
        {
            // Act
            var response = await _client.GetAsync(
                $"api/consumableTransaction/{consumableTransaction.Id}"
            );

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var result = await response.Content.ReadFromJsonAsync<ConsumableTransaction>();

            result.Should().BeEquivalentTo(consumableTransaction);
        }
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNotFound_WhenConsumableTransactionNotPresent()
    {
        var response = await _client.GetAsync($"api/consumableTransaction/{Guid.NewGuid()}");
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllConsumableTransactions_WhenConsumableTransactionsArePresent()
    {
        // Arrange
        List<ConsumableTransaction> consumableTransactions =
        [
            new ConsumableTransactionFaker(),
            new ConsumableTransactionFaker(),
        ];

        await _dbContext.ConsumableTransactions.AddRangeAsync(consumableTransactions);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"api/consumableTransaction/");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var result = await response.Content.ReadFromJsonAsync<List<ConsumableTransaction>>();

        result.Should().BeEquivalentTo(consumableTransactions);
    }

    [Fact]
    public async Task CreateForPurchaseAsync_ShouldSaveConsumableTransactions_WhenPurchaseIsCompleted()
    {
        // Arrange
        var consumables = new List<Consumable> { new ConsumableFaker(), new ConsumableFaker() };
        var entries = consumables
            .Select(consumable => new CreatePurchaseEntryDto()
            {
                AssetItem = consumable,
                Amount = consumable.Amount,
                PricePerItem = 3,
            })
            .ToList();
        var WorkingGroupFaker = new WorkingGroupFaker().Generate();
        var createPurchase = new CreatePurchaseDto()
        {
            Buyer = WorkingGroupFaker.Professor.Id, CompletionDate = DateTime.Today, Entries = entries,
        };

        // Act
        var response = await _client.PostAsync(
            $"api/purchases/{WorkingGroupFaker.Id}/",
            JsonContent.Create(createPurchase)
        );

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var consumableTransactions = await _dbContext
            .ConsumableTransactions.Where(ct =>
                consumables.Select(c => c.Id).Contains(ct.Consumable.Id)
            )
            .ToListAsync();

        consumableTransactions.Should().NotBeEmpty();
        consumableTransactions.Should().HaveCount(entries.Count);

        foreach (var entry in entries)
        {
            var transaction = consumableTransactions.Single(ct =>
                ct.Consumable.Id == entry.AssetItem.Id
            );
            transaction.AmountChange.Should().Be(-entry.Amount);
            transaction.TransactionReason.Should().Be(TransactionReasons.Purchase);
            transaction.Date.Should().Be(createPurchase.CompletionDate.Value);
        }
    }
}
