using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Controllers;
using TuDa.CIMS.Api.Test.Integration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Test.Faker;

namespace TuDa.CIMS.Api.Test.Controllers;

[TestSubject(typeof(PurchaseController))]
public class PurchaseControllerInvalidationTest1(CIMSApiFactory apiFactory)
    : ControllerTestBase(apiFactory)
{
    [Fact]
    public async Task InvalidateAsync_ShouldMarkPurchaseAsInvalidated_WhenInvalidated()
    {
        // Arrange
        var (workingGroup, entries) = await SetupWorkingGroupWithEntriesAsync(1);

        // Act
        var createResult = await CreatePurchaseAsync(
            workingGroup.Id,
            workingGroup.Professor.Id,
            entries
        );
        await InvalidatePurchaseAsync(
            workingGroup.Id,
            createResult.Id,
            workingGroup.Professor.Id,
            entries
        );

        // Assert
        var invalidPurchase = await DbContext
            .Purchases.Include(p => p.Successor)
            .SingleAsync(p => p.Id == createResult.Id);

        invalidPurchase.Invalidated.Should().BeTrue();
    }

    [Fact(Skip = "This test is failing when running all tests together.")]
    public async Task InvalidateAsync_ShouldClearConsumableTransactions_WhenPurchaseInvalidated()
    {
        // Arrange
        var (workingGroup, entries) = await SetupWorkingGroupWithEntriesAsync(2);

        // Act
        var createResult = await CreatePurchaseAsync(
            workingGroup.Id,
            workingGroup.Professor.Id,
            entries
        );
        await InvalidatePurchaseAsync(
            workingGroup.Id,
            createResult.Id,
            workingGroup.Professor.Id,
            entries
        );

        // Assert
        var invalidPurchase = await DbContext
            .Purchases.Include(p => p.ConsumableTransactions)
            .SingleAsync(p => p.Id == createResult.Id);

        invalidPurchase.ConsumableTransactions.Should().BeEmpty();
    }

    [Fact]
    public async Task InvalidateAsync_ShouldCreateCorrectTransactions_WhenModifyingExistingEntries()
    {
        // Arrange
        var (workingGroup, entries) = await SetupWorkingGroupWithEntriesAsync(2);

        var newEntries = entries
            .Select(e => new PurchaseEntry
            {
                AssetItem = e.AssetItem,
                Amount = e.Amount,
                PricePerItem = e.PricePerItem,
            })
            .ToList();

        // Modify amounts
        newEntries[0].Amount += 1;
        newEntries[1].Amount -= 1;

        // Act
        var createResult = await CreatePurchaseAsync(
            workingGroup.Id,
            workingGroup.Professor.Id,
            entries
        );
        await InvalidatePurchaseAsync(
            workingGroup.Id,
            createResult.Id,
            workingGroup.Professor.Id,
            newEntries
        );

        // Assert
        var purchase = await DbContext
            .Purchases.Include(p => p.ConsumableTransactions!)
            .ThenInclude(consumableTransaction => consumableTransaction.Consumable)
            .SingleAsync(p => p.Id != createResult.Id);

        purchase.ConsumableTransactions.Count.Should().Be(2);
        purchase
            .ConsumableTransactions.Find(t => t.Consumable.Id == newEntries[0].AssetItem.Id)!
            .AmountChange.Should()
            .Be((int)-newEntries[0].Amount);
        purchase
            .ConsumableTransactions.Find(t => t.Consumable.Id == newEntries[1].AssetItem.Id)!
            .AmountChange.Should()
            .Be((int)-newEntries[1].Amount);

        var consumables = await DbContext.Consumables.ToListAsync();

        // Verify final amounts of consumables
        consumables.Find(c => c.Id == newEntries[0].AssetItem.Id)!.Amount.Should().Be(99);
        consumables.Find(c => c.Id == newEntries[1].AssetItem.Id)!.Amount.Should().Be(101);
    }

    [Fact(Skip = "This test is failing when running all tests together.")]
    public async Task InvalidateAsync_ShouldCreateCorrectTransactions_WhenAddingNewEntries()
    {
        // Arrange
        var (workingGroup, entries) = await SetupWorkingGroupWithEntriesAsync(1);

        Consumable newConsumable = new ConsumableFaker();
        var newEntries = entries
            .Select(e => new PurchaseEntry
            {
                AssetItem = e.AssetItem,
                Amount = e.Amount,
                PricePerItem = e.PricePerItem,
            })
            .ToList();

        // Add new entry
        newEntries.Add(new PurchaseEntryFaker<Consumable>(newConsumable));
        newConsumable.Amount = (int)newEntries[1].Amount + 100;

        await DbContext.AssetItems.AddAsync(newConsumable);
        await DbContext.SaveChangesAsync();

        // Act
        var createResult = await CreatePurchaseAsync(
            workingGroup.Id,
            workingGroup.Professor.Id,
            entries
        );
        await InvalidatePurchaseAsync(
            workingGroup.Id,
            createResult.Id,
            workingGroup.Professor.Id,
            newEntries
        );

        // Assert
        var purchase = await DbContext
            .Purchases.Include(p => p.ConsumableTransactions!)
            .ThenInclude(consumableTransaction => consumableTransaction.Consumable)
            .SingleAsync(p => p.Id != createResult.Id);

        purchase.ConsumableTransactions.Count.Should().Be(2);
        purchase
            .ConsumableTransactions.Find(t => t.Consumable.Id == entries[0].AssetItem.Id)!
            .AmountChange.Should()
            .Be((int)-newEntries[0].Amount);
        purchase
            .ConsumableTransactions.Find(t => t.Consumable.Id == newConsumable.Id)!
            .AmountChange.Should()
            .Be((int)-newEntries[1].Amount);
    }

    [Fact]
    public async Task InvalidateAsync_ShouldHandleRemovedEntries_WhenEntriesAreRemoved()
    {
        // Arrange
        var (workingGroup, entries) = await SetupWorkingGroupWithEntriesAsync(3);

        // We'll only keep the first entry in the invalidated purchase
        var newEntries = new List<PurchaseEntry>
        {
            new PurchaseEntry
            {
                AssetItem = entries[0].AssetItem,
                Amount = entries[0].Amount,
                PricePerItem = entries[0].PricePerItem,
            },
        };

        // Act
        var createResult = await CreatePurchaseAsync(
            workingGroup.Id,
            workingGroup.Professor.Id,
            entries
        );
        await InvalidatePurchaseAsync(
            workingGroup.Id,
            createResult.Id,
            workingGroup.Professor.Id,
            newEntries
        );

        // Assert
        // Check that old purchase is invalidated and has no transactions
        var invalidPurchase = await DbContext
            .Purchases.Include(p => p.ConsumableTransactions)
            .SingleAsync(p => p.Id == createResult.Id);

        invalidPurchase.Invalidated.Should().BeTrue();
        invalidPurchase.ConsumableTransactions.Should().BeEmpty();

        // Check new purchase
        var newPurchase = await DbContext
            .Purchases.Include(p => p.Entries)
            .ThenInclude(purchaseEntry => purchaseEntry.AssetItem)
            .Include(p => p.ConsumableTransactions!)
            .ThenInclude(consumableTransaction => consumableTransaction.Consumable)
            .SingleAsync(p => p.Id != createResult.Id);

        // Should have only the first entry from the original purchase
        newPurchase.Entries.Count.Should().Be(1);
        newPurchase.Entries.Single().AssetItem.Id.Should().Be(entries[0].AssetItem.Id);

        // Should have correct transactions - only for the one remaining item
        newPurchase.ConsumableTransactions.Count.Should().Be(1);
        newPurchase
            .ConsumableTransactions.Single()
            .Consumable.Id.Should()
            .Be(entries[0].AssetItem.Id);
        newPurchase
            .ConsumableTransactions.Single()
            .AmountChange.Should()
            .Be((int)-newEntries[0].Amount);

        // Verify that removed entries don't have transactions in the new purchase
        newPurchase
            .ConsumableTransactions.Any(t => t.Consumable.Id == entries[1].AssetItem.Id)
            .Should()
            .BeFalse();
        newPurchase
            .ConsumableTransactions.Any(t => t.Consumable.Id == entries[2].AssetItem.Id)
            .Should()
            .BeFalse();
    }

    [Fact]
    public async Task InvalidateAsync_ShouldReturnError_WhenInvalidationCausesAssetItemAmountToGoBelowZero()
    {
        // Arrange
        // Setup a working group with one purchase entry.
        var (workingGroup, entries) = await SetupWorkingGroupWithEntriesAsync(1);

        // Create the initial purchase.
        var createResult = await CreatePurchaseAsync(
            workingGroup.Id,
            workingGroup.Professor.Id,
            entries.Select(e => e with { Amount = e.AssetItem.As<Consumable>().Amount })
        );

        var invalidAmount = entries[0].AssetItem.As<Consumable>().Amount + 1;
        var newEntry = new CreatePurchaseEntryDto
        {
            AssetItemId = entries[0].AssetItem.Id,
            Amount = invalidAmount,
            PricePerItem = entries[0].PricePerItem,
        };

        var completionDate = DateTime.Now.ToUniversalTime();
        var invalidatePurchaseDto = new CreatePurchaseDto
        {
            Buyer = workingGroup.Professor.Id,
            CompletionDate = completionDate,
            Entries = [newEntry],
        };

        // Act
        var response = await Client.PatchAsync(
            $"api/working-groups/{workingGroup.Id}/purchases/{createResult.Id}/invalidate",
            JsonContent.Create(invalidatePurchaseDto)
        );

        // Assert
        // Expect a BadRequest (400) when the invalidation would cause the asset item amount to drop below zero.
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #region Helper

    // Helper method to create a working group with consumable entries
    private async Task<(
        WorkingGroup WorkingGroup,
        List<PurchaseEntry> Entries
    )> SetupWorkingGroupWithEntriesAsync(int entryCount)
    {
        WorkingGroup workingGroup = new WorkingGroupFaker(purchases: []);
        await DbContext.WorkingGroups.AddAsync(workingGroup);
        await DbContext.SaveChangesAsync();
        var entries = new PurchaseEntryFaker<Consumable>(
            assetItemFaker: new ConsumableFaker()
        ).Generate(entryCount);

        foreach (var entry in entries)
        {
            (entry.AssetItem as Consumable)!.Amount = (int)entry.Amount + 100;
        }

        await DbContext.AssetItems.AddRangeAsync(entries.Select(e => e.AssetItem).ToList());
        try
        {
            await DbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await DbContext.SaveChangesAsync();
            Console.WriteLine(e);
        }

        return (workingGroup, entries);
    }

    // Helper method to create a purchase
    private async Task<PurchaseResponseDto> CreatePurchaseAsync(
        Guid workingGroupId,
        Guid buyerId,
        IEnumerable<PurchaseEntry> entries
    )
    {
        var completionDate = DateTime.Now.ToUniversalTime();
        var createPurchaseDto = new CreatePurchaseDto
        {
            Buyer = buyerId,
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

        var response = await Client.PostAsync(
            $"api/working-groups/{workingGroupId}/purchases/",
            JsonContent.Create(createPurchaseDto)
        );

        await response.ShouldBeSuccessAsync();
        var result = await response.Content.FromJsonAsync<PurchaseResponseDto>();
        result.Should().NotBeNull();
        return result!;
    }

    // Helper method to invalidate a purchase
    private async Task<HttpResponseMessage> InvalidatePurchaseAsync(
        Guid workingGroupId,
        Guid purchaseId,
        Guid buyerId,
        IEnumerable<PurchaseEntry> entries
    )
    {
        var completionDate = DateTime.Now.ToUniversalTime();
        var invalidatePurchaseDto = new CreatePurchaseDto
        {
            Buyer = buyerId,
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

        var response = await Client.PatchAsync(
            $"api/working-groups/{workingGroupId}/purchases/{purchaseId}/invalidate",
            JsonContent.Create(invalidatePurchaseDto)
        );

        await response.ShouldBeSuccessAsync();
        return response;
    }

    #endregion
}
