using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TuDa.CIMS.Api.Controllers;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Test.Integration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Shared.Test.Faker;

namespace TuDa.CIMS.Api.Test.Controllers;

[TestSubject(typeof(AssetItemController))]
public class AssetItemControllerTest(CIMSApiFactory apiFactory) : ControllerTestBase(apiFactory)
{
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

        await DbContext.AssetItems.AddRangeAsync(assetItems);
        await DbContext.SaveChangesAsync();

        foreach (var assetItem in assetItems)
        {
            // Act
            var response = await Client.GetAsync($"api/asset-items/{assetItem.Id}");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var result = await response.Content.ReadFromJsonAsync<AssetItem>();

            result.Should().BeEquivalentTo(assetItem);
        }
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNotFound_WhenAssetItemNotPresent()
    {
        var response = await Client.GetAsync($"api/asset-items/{Guid.NewGuid()}");
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

        await DbContext.AssetItems.AddRangeAsync(assetItems);
        await DbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"api/asset-items/");

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

        await DbContext.AssetItems.AddRangeAsync(assetItems);
        await DbContext.SaveChangesAsync();

        foreach (var assetItem in assetItems)
        {
            // Act
            var response = await Client.DeleteAsync($"api/asset-items/{assetItem.Id}");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var result = (await DbContext.AssetItems.ToListAsync());

            result.Should().NotContain(assetItem);
        }

        (await DbContext.AssetItems.AnyAsync()).Should().BeFalse();
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnNotFound_WhenAssetItemNotPresent()
    {
        var response = await Client.DeleteAsync($"api/asset-items/{Guid.NewGuid()}");
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
            new UpdateConsumableDto()
            {
                StockUpdate = new StockUpdateDto(10, TransactionReasons.Restock),
            },
            new UpdateSolventDto(),
            new UpdateGasCylinderDto(),
        ];

        await DbContext.AssetItems.AddRangeAsync(assetItems);
        await DbContext.SaveChangesAsync();

        foreach (var (assetItem, updateAssetItemDto) in assetItems.Zip(updateAssetItemDtos))
        {
            var updatedItem = assetItem with { Name = updatedName };
            updateAssetItemDto.Name = updatedName;

            // Act
            var response = await Client.PatchAsync(
                $"api/asset-items/{assetItem.Id}",
                JsonContent.Create(updateAssetItemDto)
            );

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var result = await DbContext.AssetItems.SingleAsync(i => i.Id == assetItem.Id);
            result
                .Should()
                .BeEquivalentTo(updatedItem, options => options.Excluding(item => item.UpdatedAt));
            result.UpdatedAt.Should().NotBeNull();
        }
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenAssetItemNotPresent()
    {
        var response = await Client.PatchAsync(
            $"api/asset-items/{Guid.NewGuid()}",
            JsonContent.Create<UpdateAssetItemDto>(
                new UpdateConsumableDto()
                {
                    StockUpdate = new StockUpdateDto(10, TransactionReasons.Restock),
                }
            )
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

        await DbContext.AssetItems.AddRangeAsync(assetItems);
        await DbContext.SaveChangesAsync();

        var response1 = await Client.GetAsync("api/asset-items/paginated?pageNumber=1&pageSize=2");
        var response2 = await Client.GetAsync("api/asset-items/paginated?pageNumber=2&pageSize=2");
        var response3 = await Client.GetAsync("api/asset-items/paginated?pageNumber=3&pageSize=2");

        response1.IsSuccessStatusCode.Should().BeTrue();
        response2.IsSuccessStatusCode.Should().BeTrue();

        response3.IsSuccessStatusCode.Should().BeFalse();
        response3.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var result1 = await response1.Content.FromJsonAsync<List<AssetItem>>();
        var result2 = await response2.Content.FromJsonAsync<List<AssetItem>>();

        result1.Should().BeEquivalentTo(assetItems[..2]);
        result2.Should().BeEquivalentTo(assetItems[2..4]);
    }

    [Fact]
    public async Task UpdateAsync_ShouldCreateConsumableTransaction_WhenAssetItemIsUpdated()
    {
        // Arrange
        var consumableFaker = new ConsumableFaker().Generate();
        consumableFaker.Amount = 100;
        List<AssetItem> assetItems =
        [
            new ChemicalFaker(),
            consumableFaker,
            new SolventFaker(),
            new GasCylinderFaker(),
        ];

        const string updatedName = "Updated";
        const int updatedAmount = 15;

        List<UpdateAssetItemDto> updateAssetItemDtos =
        [
            new UpdateChemicalDto(),
            new UpdateConsumableDto
            {
                Name = updatedName,
                StockUpdate = new StockUpdateDto(updatedAmount, TransactionReasons.Restock),
            },
            new UpdateSolventDto(),
            new UpdateGasCylinderDto(),
        ];

        await DbContext.AssetItems.AddRangeAsync(assetItems);
        await DbContext.SaveChangesAsync();

        var previousAmount = consumableFaker.Amount;
        foreach (var (assetItem, updateAssetItemDto) in assetItems.Zip(updateAssetItemDtos))
        {
            var updatedItem = assetItem with { Name = updatedName };
            updateAssetItemDto.Name = updatedName;

            // Act
            var response = await Client.PatchAsync(
                $"api/asset-items/{assetItem.Id}",
                JsonContent.Create(updateAssetItemDto)
            );

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var result = await DbContext.AssetItems.SingleAsync(i => i.Id == assetItem.Id);
            result
                .Should()
                .BeEquivalentTo(updatedItem, options => options.Excluding(item => item.UpdatedAt));
            result.UpdatedAt.Should().NotBeNull();

            if (assetItem is Consumable)
            {
                var transaction = await DbContext
                    .ConsumableTransactions.Where(t => t.Consumable.Id == assetItem.Id)
                    .OrderByDescending(t => t.Date)
                    .FirstOrDefaultAsync();

                transaction.Should().NotBeNull();
                transaction!.AmountChange.Should().Be(updatedAmount - previousAmount);
                transaction.TransactionReason.Should().Be(TransactionReasons.Restock);
            }
        }
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateConsumableTransaction_WhenConsumableIsCreated()
    {
        // Arrange
        var createConsumable = new CreateConsumableDto
        {
            ItemNumber = "12345",
            Name = "Test Consumable",
            Note = "Test Note",
            Price = 50,
            Shop = "Test Shop",
            Room = Rooms.G27,
            Amount = 10,
            Manufacturer = "Test Manufacturer",
            SerialNumber = "ABC123",
        };

        // Act
        var response = await Client.PostAsync(
            "api/asset-items",
            JsonContent.Create<CreateAssetItemDto>(createConsumable)
        );

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var createdConsumable = await DbContext.Consumables.SingleAsync();
        createdConsumable.Should().NotBeNull();
        createdConsumable.Amount.Should().Be(createConsumable.Amount);

        var transaction = await DbContext
            .ConsumableTransactions.Where(t => t.Consumable.Id == createdConsumable.Id)
            .SingleAsync();

        transaction.Should().NotBeNull();
        transaction.AmountChange.Should().Be(createConsumable.Amount);
        transaction.TransactionReason.Should().Be(TransactionReasons.Init);
    }
}
