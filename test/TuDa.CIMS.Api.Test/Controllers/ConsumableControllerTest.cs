using TuDa.CIMS.Api.Controllers;
using TuDa.CIMS.Api.Test.Integration;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Shared.Models;
using TuDa.CIMS.Shared.Test.Faker;

namespace TuDa.CIMS.Api.Test.Controllers;

[TestSubject(typeof(ConsumableController))]
public class ConsumableControllerTest(CIMSApiFactory apiFactory) : ControllerTestBase(apiFactory)
{
    private const string BaseRoute = "api/consumables";
    const int DefaultYear = 2000;

    [Fact]
    public async Task GetStatisticsForYear_ShouldProduceCorrectStatistics_WhenAllFromSameYearAndConsumable()
    {
        var (consumable, transactions) = await SeedConsumableWithTransactionsAsync();

        var expected = TransactionsToStatistics(consumable, transactions);

        var response = await Client.GetAsync(
            $"{BaseRoute}/{consumable.Id}/statistics/{DefaultYear}"
        );

        response.IsSuccessStatusCode.Should().BeTrue();

        var statistics = await response.Content.FromJsonAsync<ConsumableStatistics>();

        statistics.Should().Be(expected);
    }

    [Fact]
    public async Task GetStatisticsForYear_ShouldProduceCorrectStatistics_WhenAllSameConsumableButDifferentYear()
    {
        var (consumable, transactions) = await SeedConsumableWithTransactionsAsync();
        await SeedTransactionsWithYearAsync(consumable, DefaultYear + 1);

        var expected = TransactionsToStatistics(consumable, transactions);

        var response = await Client.GetAsync(
            $"{BaseRoute}/{consumable.Id}/statistics/{DefaultYear}"
        );

        response.IsSuccessStatusCode.Should().BeTrue();

        var statistics = await response.Content.FromJsonAsync<ConsumableStatistics>();

        statistics.Should().Be(expected);
    }

    [Fact]
    public async Task GetStatisticsForYear_ShouldProduceCorrectStatistics_WhenAllSameYearButDifferentConsumable()
    {
        var (consumable, transactions) = await SeedConsumableWithTransactionsAsync();
        await SeedConsumableWithTransactionsAsync();

        var expected = TransactionsToStatistics(consumable, transactions);

        var response = await Client.GetAsync(
            $"{BaseRoute}/{consumable.Id}/statistics/{DefaultYear}"
        );

        response.IsSuccessStatusCode.Should().BeTrue();

        var statistics = await response.Content.FromJsonAsync<ConsumableStatistics>();

        statistics.Should().Be(expected);
    }

    [Fact]
    public async Task GetStatisticsForYear_ShouldProduceCorrectStatistics_WhenMultipleConsumableAndYears()
    {
        var (consumable, transactions) = await SeedConsumableWithTransactionsAsync();
        await SeedTransactionsWithYearAsync(consumable, DefaultYear + 1);
        await SeedConsumableWithTransactionsAsync();

        var expected = TransactionsToStatistics(consumable, transactions);

        var response = await Client.GetAsync(
            $"{BaseRoute}/{consumable.Id}/statistics/{DefaultYear}"
        );

        response.IsSuccessStatusCode.Should().BeTrue();

        var statistics = await response.Content.FromJsonAsync<ConsumableStatistics>();

        statistics.Should().Be(expected);
    }

    [Fact]
    public async Task GetStatisticsForYear_ShouldSkipConsumableTransaction_WhenTransactionReasonIsInit()
    {
        var (consumable, transactions) = await SeedConsumableWithTransactionsAsync();
        await SeedTransactionsAsync(
            consumable,
            transaction => transaction with { TransactionReason = TransactionReasons.Init }
        );

        var expected = TransactionsToStatistics(consumable, transactions);

        var response = await Client.GetAsync(
            $"{BaseRoute}/{consumable.Id}/statistics/{DefaultYear}"
        );

        response.IsSuccessStatusCode.Should().BeTrue();

        var statistics = await response.Content.FromJsonAsync<ConsumableStatistics>();

        statistics.Should().Be(expected);
    }

    #region Helper

    private async Task<(
        Consumable consumable,
        List<ConsumableTransaction> transactions
    )> SeedConsumableWithTransactionsAsync(int year = DefaultYear)
    {
        var consumable = await SeedConsumableAsync();
        var transactions = await SeedTransactionsWithYearAsync(consumable, year);

        return (consumable, transactions);
    }

    private async Task<Consumable> SeedConsumableAsync()
    {
        var consumable = new ConsumableFaker();
        await DbContext.Consumables.AddAsync(consumable);
        await DbContext.SaveChangesAsync();
        return consumable;
    }

    private async Task<List<ConsumableTransaction>> SeedTransactionsAsync(
        Consumable consumable,
        Func<ConsumableTransaction, ConsumableTransaction> action
    )
    {
        var transactions = new ConsumableTransactionFaker(consumable)
            .Generate(5)
            .Select(action)
            .ToList();

        await DbContext.ConsumableTransactions.AddRangeAsync(transactions);
        await DbContext.SaveChangesAsync();
        return transactions;
    }

    private Task<List<ConsumableTransaction>> SeedTransactionsWithYearAsync(
        Consumable consumable,
        int year
    ) =>
        SeedTransactionsAsync(
            consumable,
            transaction =>
                transaction with
                {
                    Date = new DateTime(
                        year,
                        transaction.Date.Month,
                        transaction.Date.Day,
                        0,
                        0,
                        0,
                        DateTimeKind.Utc
                    ),
                }
        );

    private static ConsumableStatistics TransactionsToStatistics(
        Consumable consumable,
        IEnumerable<ConsumableTransaction> transactions
    )
    {
        var statistics = transactions.Aggregate(
            new ConsumableStatistics(),
            (statistics, transaction) =>
            {
                if (transaction.AmountChange > 0)
                {
                    statistics.TotalAdded += transaction.AmountChange;
                }
                else
                {
                    statistics.TotalRemoved -= transaction.AmountChange;
                }

                return statistics;
            }
        );
        statistics.CurrentAmount = consumable.Amount;
        statistics.PreviousYearAmount =
            consumable.Amount - statistics.TotalAdded + statistics.TotalRemoved;
        return statistics;
    }

    #endregion
}
