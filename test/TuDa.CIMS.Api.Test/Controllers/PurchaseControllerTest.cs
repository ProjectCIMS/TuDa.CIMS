using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TuDa.CIMS.Api.Controllers;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Test.Integration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Test.Faker;

namespace TuDa.CIMS.Api.Test.Controllers;

[TestSubject(typeof(PurchaseController))]
public class PurchaseControllerTest : IClassFixture<CIMSApiFactory>
{
    private readonly HttpClient _client;
    private readonly CIMSDbContext _dbContext;

    public PurchaseControllerTest(CIMSApiFactory apiFactory)
    {
        _client = apiFactory.CreateClient();

        var scope = apiFactory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<CIMSDbContext>();

        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.Migrate();
    }

    [Fact]
    public async Task GetAsync_ShouldReturnPurchase_WhenPurchasePresent()
    {
        // Arrange
        WorkingGroup workingGroup = new WorkingGroupFaker();

        await _dbContext.WorkingGroups.AddRangeAsync(workingGroup);
        await _dbContext.SaveChangesAsync();

        foreach (var purchase in workingGroup.Purchases)
        {
            // Act
            var response = await _client.GetAsync($"api/working-groups/{workingGroup.Id}/purchases/{purchase.Id}");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var result = await response.Content.FromJsonAsync<Purchase>();

            result.Should().BeEquivalentTo(purchase);
        }
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNotFound_WhenPurchaseNotPresent()
    {
        WorkingGroup workingGroup = new WorkingGroupFaker(purchases: []);
        var response = await _client.GetAsync($"api/working-groups/{workingGroup.Id}/purchases/{Guid.NewGuid()}");
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPurchases_WhenPurchasesArePresent()
    {
        // Arrange
        WorkingGroup workingGroup = new WorkingGroupFaker();

        await _dbContext.WorkingGroups.AddRangeAsync(workingGroup);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"api/working-groups/{workingGroup.Id}/purchases");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var result = await response.Content.FromJsonAsync<List<Purchase>>();

        result.Should().BeEquivalentTo(workingGroup.Purchases);
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemovePurchase_WhenPurchasePresent()
    {
        // Arrange
        WorkingGroup workingGroup = new WorkingGroupFaker();
        List<Purchase> purchases = new PurchaseFaker(workingGroup).GenerateBetween(2, 5);
        workingGroup.Purchases = [.. purchases];

        await _dbContext.WorkingGroups.AddAsync(workingGroup);
        await _dbContext.SaveChangesAsync();

        foreach (var purchase in purchases)
        {
            // Act
            var response = await _client.DeleteAsync(
                $"api/working-groups/{workingGroup.Id}/purchases/{purchase.Id}"
            );

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var result = (await _dbContext.Purchases.ToListAsync());

            result.Should().NotContain(purchase);
        }

        (await _dbContext.Purchases.AnyAsync()).Should().BeFalse();
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnNotFound_WhenPurchaseNotPresent()
    {
        WorkingGroup workingGroup = new WorkingGroupFaker(purchases: []);

        await _dbContext.WorkingGroups.AddAsync(workingGroup);
        await _dbContext.SaveChangesAsync();

        var response = await _client.DeleteAsync(
            $"api/working-groups/{workingGroup.Id}/purchases/{Guid.NewGuid()}"
        );
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreatePurchase_WhenWorkingGroupPresent()
    {
        // Arrange
        WorkingGroup workingGroup = new WorkingGroupFaker(purchases: []);
        Consumable assetItem = new ConsumableFaker();

        await _dbContext.WorkingGroups.AddRangeAsync(workingGroup);
        await _dbContext.AssetItems.AddAsync(assetItem);
        await _dbContext.SaveChangesAsync();

        var completionDate = DateTime.Now.ToUniversalTime();
        var entries = new PurchaseEntryFaker<Consumable>(assetItem).GenerateBetween(1, 10);

        var createPurchase = new CreatePurchaseDto
        {
            Buyer = workingGroup.Professor.Id,
            CompletionDate = completionDate,
            Entries = entries
                .Select(entry => new CreatePurchaseEntryDto
                {
                    AssetItemId = entry.AssetItem.Id,
                    Amount = (int)entry.Amount,
                    PricePerItem = entry.PricePerItem,
                })
                .ToList(),
        };

        // Set Amount to amount purchased to ensure no error is returned
        assetItem.Amount = entries.Aggregate(0, (i, entry) => i + (int)entry.Amount);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.PostAsync(
            $"api/working-groups/{workingGroup.Id}/purchases",
            JsonContent.Create(createPurchase)
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await _dbContext
            .Purchases.Include(p => p.Entries)
            .ThenInclude(e => e.AssetItem)
            .Include(p => p.Buyer)
            .SingleAsync();
        result.Buyer.Should().BeEquivalentTo(workingGroup.Professor);
        result.CompletionDate.Should().Be(completionDate);
        result.Entries.Should()
            .BeEquivalentTo(entries, options => options.Excluding(e => e.Id));
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnNotFound_WhenWorkingGroupNotPresent()
    {
        var response = await _client.PostAsync(
            $"api/working-groups/{Guid.NewGuid()}/purchases",
            JsonContent.Create(new CreatePurchaseDto() { Buyer = Guid.NewGuid() })
        );

        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
