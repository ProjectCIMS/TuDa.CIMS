using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Test.Faker;

namespace TuDa.CIMS.Api.Test.Integration;

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
        // Arange
        Chemical assetItem = new ChemicalFaker();

        await _dbContext.AssetItems.AddAsync(assetItem);
        await _dbContext.SaveChangesAsync();

        var response = await _client.GetAsync($"api/asset-items/{assetItem.Id}");

        response.IsSuccessStatusCode.Should().BeTrue();

        var result = await response.Content.ReadFromJsonAsync<AssetItem>();

        result.Should().BeEquivalentTo(assetItem);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAssetItems_WhenAssetItemsArePresent()
    {
        // Arange
        List<AssetItem> assetItems =
        [
            new ConsumableFaker(),
            new ChemicalFaker(),
            new SolventFaker(),
            new GasCylinderFaker(),
        ];

        await _dbContext.AssetItems.AddRangeAsync(assetItems);
        await _dbContext.SaveChangesAsync();

        var response = await _client.GetAsync($"api/asset-items/");

        response.IsSuccessStatusCode.Should().BeTrue();

        var result = await response.Content.ReadFromJsonAsync<List<AssetItem>>();

        result.Should().BeEquivalentTo(assetItems);
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveAssetItem_WhenAssetItemPresent()
    {
        // Arange
        Chemical assetItem = new ChemicalFaker();

        await _dbContext.AssetItems.AddAsync(assetItem);
        await _dbContext.SaveChangesAsync();

        var response = await _client.DeleteAsync($"api/asset-items/{assetItem.Id}");

        response.IsSuccessStatusCode.Should().BeTrue();

        var result = (await _dbContext.AssetItems.ToListAsync());

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAssetItem_WhenAssetItemIsUpdated()
    {
        // Arange
        Chemical assetItem = new ChemicalFaker();

        await _dbContext.AssetItems.AddAsync(assetItem);
        await _dbContext.SaveChangesAsync();

        var updatedItem = assetItem with { Name = "Updated" };
        var updateDto = new UpdateChemicalDto { Name = "Updated" };

        var response = await _client.PatchAsync(
            $"api/asset-items/{assetItem.Id}",
            JsonContent.Create<UpdateAssetItemDto>(updateDto)
        );

        response.IsSuccessStatusCode.Should().BeTrue();

        var result = await _dbContext.AssetItems.SingleAsync(i => i.Id == assetItem.Id);

        result.Should().BeEquivalentTo(updatedItem);
    }
}
