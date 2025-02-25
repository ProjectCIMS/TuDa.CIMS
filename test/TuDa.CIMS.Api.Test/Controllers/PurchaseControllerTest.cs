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
[Collection("Sequential")]
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

    [Fact()]
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

    [Fact(Skip = "This test is failing when running all tests together.")]
    public async Task RemoveAsync_ShouldRemovePurchase_WhenPurchasePresent()
    {
        // Arrange
        WorkingGroup workingGroup = new WorkingGroupFaker();
        await DbContext.WorkingGroups.AddAsync(workingGroup);
        await DbContext.SaveChangesAsync();

        var purchases = await DbContext.Purchases.ToListAsync();

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

    [Fact(Skip = "This test is failing when running all tests together.")]
    public async Task RemoveAsync_ShouldReturnNotFound_WhenPurchaseNotPresent()
    {
        WorkingGroup workingGroup = new WorkingGroupFaker();
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

    [Fact(Skip = "This test is failing when running all tests together.")]
    public async Task RetrieveSignatureAsync_ShouldReturnBase64Signature_WhenPurchaseExists()
    {
        // Arrange
        WorkingGroup workingGroup = new WorkingGroupFaker();
        await DbContext.WorkingGroups.AddAsync(workingGroup);
        await DbContext.SaveChangesAsync();

        var purchases = await DbContext.Purchases.ToListAsync();

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
}
