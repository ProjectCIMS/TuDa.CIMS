using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TuDa.CIMS.Api.Controllers;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Test.Integration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Extensions;
using TuDa.CIMS.Shared.Test.Faker;

namespace TuDa.CIMS.Api.Test.Controllers;

[TestSubject(typeof(PurchaseController))]
public class PurchaseControllerTest(CIMSApiFactory apiFactory) : ControllerTestBase(apiFactory)
{
    [Fact]
    public async Task GetAsync_ShouldReturnPurchase_WhenPurchasePresent()
    {
        // Arrange
        WorkingGroup workingGroup = new WorkingGroupFaker();

        await DbContext.WorkingGroups.AddRangeAsync(workingGroup);
        await DbContext.SaveChangesAsync();

        foreach (var purchase in workingGroup.Purchases)
        {
            // Act
            var response = await Client.GetAsync(
                $"api/working-groups/{workingGroup.Id}/purchases/{purchase.Id}"
            );

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var result = await response.Content.FromJsonAsync<PurchaseResponseDto>();

            result.Should().BeEquivalentTo(purchase.ToResponseDto());
        }
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNotFound_WhenPurchaseNotPresent()
    {
        WorkingGroup workingGroup = new WorkingGroupFaker(purchases: []);
        var response = await Client.GetAsync(
            $"api/working-groups/{workingGroup.Id}/purchases/{Guid.NewGuid()}"
        );
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPurchases_WhenPurchasesArePresent()
    {
        // Arrange
        WorkingGroup workingGroup = new WorkingGroupFaker();

        await DbContext.WorkingGroups.AddRangeAsync(workingGroup);
        await DbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"api/working-groups/{workingGroup.Id}/purchases");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var result = await response.Content.FromJsonAsync<List<PurchaseResponseDto>>();

        result.Should().BeEquivalentTo(workingGroup.Purchases.ToResponseDtos());
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemovePurchase_WhenPurchasePresent()
    {
        // Arrange
        WorkingGroup workingGroup = new WorkingGroupFaker();
        List<Purchase> purchases = new PurchaseFaker(workingGroup).GenerateBetween(2, 5);
        workingGroup.Purchases = [.. purchases];

        await DbContext.WorkingGroups.AddAsync(workingGroup);
        await DbContext.SaveChangesAsync();

        foreach (var purchase in purchases)
        {
            // Act
            var response = await Client.DeleteAsync(
                $"api/working-groups/{workingGroup.Id}/purchases/{purchase.Id}"
            );

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var result = (await DbContext.Purchases.ToListAsync());

            result.Should().NotContain(purchase);
        }

        (await DbContext.Purchases.AnyAsync()).Should().BeFalse();
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnNotFound_WhenPurchaseNotPresent()
    {
        WorkingGroup workingGroup = new WorkingGroupFaker(purchases: []);

        await DbContext.WorkingGroups.AddAsync(workingGroup);
        await DbContext.SaveChangesAsync();

        var response = await Client.DeleteAsync(
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

        await DbContext.WorkingGroups.AddRangeAsync(workingGroup);
        await DbContext.AssetItems.AddAsync(assetItem);
        await DbContext.SaveChangesAsync();

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
        await DbContext.SaveChangesAsync();

        // Act
        var response = await Client.PostAsync(
            $"api/working-groups/{workingGroup.Id}/purchases",
            JsonContent.Create(createPurchase)
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await DbContext
            .Purchases.Include(p => p.Entries)
            .ThenInclude(e => e.AssetItem)
            .Include(p => p.Buyer)
            .SingleAsync();
        result.Buyer.Should().BeEquivalentTo(workingGroup.Professor);
        result.CompletionDate.Should().Be(completionDate);
        result.Entries.Should().BeEquivalentTo(entries, options => options.Excluding(e => e.Id));
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnNotFound_WhenWorkingGroupNotPresent()
    {
        var response = await Client.PostAsync(
            $"api/working-groups/{Guid.NewGuid()}/purchases",
            JsonContent.Create(new CreatePurchaseDto() { Buyer = Guid.NewGuid() })
        );

        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RetrieveSignatureAsync_ShouldReturnBase64Signature_WhenPurchaseExists()
    {
        // Arrange
        WorkingGroup workingGroup = new WorkingGroupFaker();
        List<Purchase> purchases = new PurchaseFaker(workingGroup).GenerateBetween(2, 5);
        workingGroup.Purchases = [.. purchases];

        await DbContext.WorkingGroups.AddAsync(workingGroup);
        await DbContext.SaveChangesAsync();

        foreach (var purchase in purchases)
        {
            // Act
            var response = await Client.GetAsync(
                $"api/working-groups/{workingGroup.Id}/purchases/{purchase.Id}/signature"
            );

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            string base64Signature = await response.Content.ReadAsStringAsync();
            string expectedBase64 = Convert.ToBase64String(purchase.Signature);

            base64Signature.Should().Be(expectedBase64);
        }
    }

    [Fact]
    public async Task RetrieveSignatureAsync_ShouldReturnNotFound_WhenPurchaseDoesNotExist()
    {
        // Arrange
        var workingGroup = new WorkingGroupFaker().Generate();
        await DbContext.WorkingGroups.AddAsync(workingGroup);
        await DbContext.SaveChangesAsync();

        var nonExistentPurchaseId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync(
            $"api/working-groups/{workingGroup.Id}/purchases/{nonExistentPurchaseId}/signature"
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task InvalidateAsync_ShouldCorrectlyInvalidate_WhenPurchasePresent()
    {
        // Arrange
        WorkingGroup workingGroup = new WorkingGroupFaker(purchases: []);
        var entries = new PurchaseEntryFaker<Consumable>(
            assetItemFaker: new ConsumableFaker()
        ).Generate(4);

        var newEntries = entries.Take(3).ToList();
        newEntries[0].Amount += 1;
        newEntries[1].Amount -= 1;

        Consumable newConsumable = new ConsumableFaker();

        newEntries.Add(new PurchaseEntryFaker<Consumable>(newConsumable));
        newConsumable.Amount = (int)newEntries[2].Amount + 100;

        foreach (var entry in entries)
        {
            (entry.AssetItem as Consumable)!.Amount = (int)entry.Amount + 100;
        }

        await DbContext.WorkingGroups.AddAsync(workingGroup);
        await DbContext.AssetItems.AddRangeAsync(entries.Select(e => e.AssetItem));
        await DbContext.AssetItems.AddAsync(newConsumable);
        await DbContext.SaveChangesAsync();

        var completionDate = DateTime.Now.ToUniversalTime();
        var initialPurchase = new CreatePurchaseDto
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

        var invalidatePurchase = new CreatePurchaseDto
        {
            Buyer = workingGroup.Professor.Id,
            CompletionDate = completionDate,
            Entries = newEntries
                .Select(entry => new CreatePurchaseEntryDto
                {
                    AssetItemId = entry.AssetItem.Id,
                    Amount = (int)entry.Amount,
                    PricePerItem = entry.PricePerItem,
                })
                .ToList(),
        };

        // Act
        var createResponse = await Client.PostAsync(
            $"api/working-groups/{workingGroup.Id}/purchases/",
            JsonContent.Create(initialPurchase)
        );

        await createResponse.ShouldBeSuccessAsync();
        var createResult = await createResponse.Content.FromJsonAsync<PurchaseResponseDto>();

        createResult.Should().NotBeNull();

        var invalidateResponse = await Client.PatchAsync(
            $"api/working-groups/{workingGroup.Id}/purchases/{createResult!.Id}/invalidate",
            JsonContent.Create(invalidatePurchase)
        );

        // Assert
        await invalidateResponse.ShouldBeSuccessAsync();

        var invalidPurchase = await DbContext
            .Purchases.Include(p => p.Successor)
            .Include(purchase => purchase.ConsumableTransactions)
            .SingleAsync(p => p.Id == createResult.Id);

        var purchase = await DbContext
            .Purchases.Include(p => p.Entries)
            .ThenInclude(e => e.AssetItem)
            .Include(purchase => purchase.ConsumableTransactions!)
            .ThenInclude(consumableTransaction => consumableTransaction.Consumable)
            .SingleAsync(p => p.Id != createResult.Id);

        invalidPurchase.Invalidated.Should().BeTrue();
        invalidPurchase.ConsumableTransactions.Should().BeEmpty();

        purchase.ConsumableTransactions.Count.Should().Be(4);
        purchase
            .ConsumableTransactions.Find(t => t.Consumable.Id == entries[0].AssetItem.Id)!
            .AmountChange.Should()
            .Be((int)-newEntries[0].Amount);
        purchase
            .ConsumableTransactions.Find(t => t.Consumable.Id == entries[1].AssetItem.Id)!
            .AmountChange.Should()
            .Be((int)-newEntries[1].Amount);

        purchase
            .ConsumableTransactions.Find(t => t.Consumable.Id == entries[2].AssetItem.Id)!
            .AmountChange.Should()
            .Be((int)-newEntries[2].Amount);

        purchase
            .ConsumableTransactions.Find(t => t.Consumable.Id == newConsumable.Id)!
            .AmountChange.Should()
            .Be((int)-newEntries[3].Amount);
    }
}
