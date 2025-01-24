using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TuDa.CIMS.Api.Controllers;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Test.Integration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Test.Faker;

namespace TuDa.CIMS.Api.Test.Controllers;

[TestSubject(typeof(AssetItemController))]
public class AssetItemControllerTest : IClassFixture<CIMSApiFactory>
{
    private readonly HttpClient _client;
    private readonly CIMSDbContext _dbContext;

    public AssetItemControllerTest(CIMSApiFactory apiFactory)
    {
        _client = apiFactory.CreateClient();

        var scope = apiFactory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<CIMSDbContext>();

        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.Migrate();
    }

    [Fact]
    public async Task GetAsync_ShouldReturnAssetItem_WhenAssetItemPresent()
    {
        // Arrange
        List<AssetItem> assetItems =
        [
            new ConsumableFaker(),
            new ChemicalFaker(),
            new SolventFaker(),
            new GasCylinderFaker(),
        ];

        await _dbContext.AssetItems.AddRangeAsync(assetItems);
        await _dbContext.SaveChangesAsync();

        foreach (var assetItem in assetItems)
        {
            // Act
            var response = await _client.GetAsync($"api/asset-items/{assetItem.Id}");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var result = await response.Content.ReadFromJsonAsync<AssetItem>();

            result.Should().BeEquivalentTo(assetItem);
        }
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNotFound_WhenAssetItemNotPresent()
    {
        var response = await _client.GetAsync($"api/asset-items/{Guid.NewGuid()}");
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAssetItems_WhenAssetItemsArePresent()
    {
        // Arrange
        List<AssetItem> assetItems =
        [
            new ConsumableFaker(),
            new ChemicalFaker(),
            new SolventFaker(),
            new GasCylinderFaker(),
        ];

        await _dbContext.AssetItems.AddRangeAsync(assetItems);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"api/asset-items/");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var result = await response.Content.FromJsonAsync<List<AssetItem>>();

        result.Should().BeEquivalentTo(assetItems);
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveAssetItem_WhenAssetItemPresent()
    {
        // Arrange
        List<AssetItem> assetItems =
        [
            new ChemicalFaker(),
            new ConsumableFaker(),
            new SolventFaker(),
            new GasCylinderFaker(),
        ];

        await _dbContext.AssetItems.AddRangeAsync(assetItems);
        await _dbContext.SaveChangesAsync();

        foreach (var assetItem in assetItems)
        {
            // Act
            var response = await _client.DeleteAsync($"api/asset-items/{assetItem.Id}");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var result = (await _dbContext.AssetItems.ToListAsync());

            result.Should().NotContain(assetItem);
        }

        (await _dbContext.AssetItems.AnyAsync()).Should().BeFalse();
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnNotFound_WhenAssetItemNotPresent()
    {
        var response = await _client.DeleteAsync($"api/asset-items/{Guid.NewGuid()}");
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAssetItem_WhenAssetItemIsUpdated()
    {
        // Arrange
        List<AssetItem> assetItems =
        [
            new ChemicalFaker(),
            new ConsumableFaker(),
            new SolventFaker(),
            new GasCylinderFaker(),
        ];
        const string updatedName = "Updated";
        List<UpdateAssetItemDto> updateAssetItemDtos =
        [
            new UpdateChemicalDto(),
            new UpdateConsumableDto(),
            new UpdateSolventDto(),
            new UpdateGasCylinderDto(),
        ];

        await _dbContext.AssetItems.AddRangeAsync(assetItems);
        await _dbContext.SaveChangesAsync();

        foreach (var (assetItem, updateAssetItemDto) in assetItems.Zip(updateAssetItemDtos))
        {
            var updatedItem = assetItem with { Name = updatedName };
            updateAssetItemDto.Name = updatedName;

            // Act
            var response = await _client.PatchAsync(
                $"api/asset-items/{assetItem.Id}",
                JsonContent.Create(updateAssetItemDto)
            );

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var result = await _dbContext.AssetItems.SingleAsync(i => i.Id == assetItem.Id);
            result
                .Should()
                .BeEquivalentTo(updatedItem, options => options.Excluding(item => item.UpdatedAt));
            result.UpdatedAt.Should().NotBeNull();
        }
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenAssetItemNotPresent()
    {
        var response = await _client.PatchAsync(
            $"api/asset-items/{Guid.NewGuid()}",
            JsonContent.Create<UpdateAssetItemDto>(new UpdateConsumableDto())
        );
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetPaginatedAsync_ShouldReturnCorrectPagination_WhenPaginationNecessary()
    {
        List<AssetItem> assetItems =
        [
            new ChemicalFaker(),
            new ConsumableFaker(),
            new SolventFaker(),
            new GasCylinderFaker(),
        ];

        await _dbContext.AssetItems.AddRangeAsync(assetItems);
        await _dbContext.SaveChangesAsync();

        var response1 = await _client.GetAsync("api/asset-items/paginated?pageNumber=1&pageSize=2");
        var response2 = await _client.GetAsync("api/asset-items/paginated?pageNumber=2&pageSize=2");
        var response3 = await _client.GetAsync("api/asset-items/paginated?pageNumber=3&pageSize=2");

        response1.IsSuccessStatusCode.Should().BeTrue();
        response2.IsSuccessStatusCode.Should().BeTrue();

        response3.IsSuccessStatusCode.Should().BeFalse();
        response3.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var result1 = await response1.Content.FromJsonAsync<List<AssetItem>>();
        var result2 = await response2.Content.FromJsonAsync<List<AssetItem>>();

        result1.Should().BeEquivalentTo(assetItems[..2]);
        result2.Should().BeEquivalentTo(assetItems[2..4]);
    }
}
