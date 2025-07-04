﻿using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Test.Integration;
using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Test.Faker;

namespace TuDa.CIMS.Api.Test.Controllers;

public class ConsumableTransactionTest(CIMSApiFactory apiFactory) : ControllerTestBase(apiFactory)
{
    [Fact]
    public async Task GetAsync_ShouldReturnConsumableTransaction_WhenConsumableTransactionPresent()
    {
        // Arrange
        List<ConsumableTransaction> consumableTransactions =
        [
            new ConsumableTransactionFaker(),
            new ConsumableTransactionFaker(),
        ];

        await DbContext.ConsumableTransactions.AddRangeAsync(consumableTransactions);
        await DbContext.SaveChangesAsync();

        foreach (var consumableTransaction in consumableTransactions)
        {
            // Act
            var response = await Client.GetAsync(
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
        var response = await Client.GetAsync($"api/consumableTransaction/{Guid.NewGuid()}");
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

        await DbContext.ConsumableTransactions.AddRangeAsync(consumableTransactions);
        await DbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"api/consumableTransaction/");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var result = await response.Content.FromJsonAsync<List<ConsumableTransaction>>();

        result.Should().BeEquivalentTo(consumableTransactions);
    }

    [Fact]
    public async Task CreateForPurchaseAsync_ShouldSaveConsumableTransactions_WhenPurchaseIsCompleted()
    {
        // Arrange
        var consumables = new List<Consumable> { new ConsumableFaker(), new ConsumableFaker() };

        var WorkingGroupFaker = new WorkingGroupFaker(purchases: []).Generate();

        await DbContext.WorkingGroups.AddAsync(WorkingGroupFaker);
        await DbContext.AssetItems.AddRangeAsync(consumables);
        await DbContext.SaveChangesAsync();

        var entries = consumables
            .Select(consumable => new CreatePurchaseEntryDto()
            {
                AssetItemId = consumable.Id,
                Amount = consumable.Amount,
                PricePerItem = 3,
            })
            .ToList();

        var createPurchase = new CreatePurchaseDto()
        {
            Buyer = WorkingGroupFaker.Professor.Id,
            CompletionDate = DateTime.Today.ToUniversalTime(),
            Entries = entries,
        };

        // Act
        var response = await Client.PostAsync(
            $"api/working-groups/{WorkingGroupFaker.Id}/purchases",
            JsonContent.Create(createPurchase)
        );

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var consumableTransactions = await DbContext
            .ConsumableTransactions.Include(consumableTransaction =>
                consumableTransaction.Consumable
            )
            .ToListAsync();

        consumableTransactions.Should().NotBeEmpty();
        consumableTransactions.Should().HaveCount(entries.Count);

        foreach (var (entry, transaction) in entries.Zip(consumableTransactions))
        {
            entry.Amount.Should().Be(-transaction.AmountChange);
            entry.AssetItemId.Should().Be(transaction.Consumable.Id);
        }
    }
}
