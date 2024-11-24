using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using OpenTelemetry.Trace;
using TuDa.CIMS.Api;

namespace TuDa.CIMS.MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<Worker> logger
) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = s_activitySource.StartActivity(
            "Migrating database",
            ActivityKind.Client
        );

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(
                stoppingToken
            );
            if (!pendingMigrations.Any())
            {
                logger.LogInformation("No pending Migrations");
                hostApplicationLifetime.StopApplication();
                return;
            }

            await EnsureDatabaseAsync(dbContext, stoppingToken);
            await RunMigrationAsync(dbContext, stoppingToken);
            await SeedDataAsync(dbContext, stoppingToken);
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task EnsureDatabaseAsync(
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Create the database if it does not exist.
            // Do this first so there is then a database to start a transaction against.
            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                await dbCreator.CreateAsync(cancellationToken);
            }
        });
    }

    private static async Task RunMigrationAsync(
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await using var transaction = await dbContext.Database.BeginTransactionAsync(
                cancellationToken
            );
            await dbContext.Database.MigrateAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }

    private static async Task SeedDataAsync(
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        // TODO: Seed DB
        /*ItemType firstItemType = new() { Name = "Default Type" };

        Place firstPlace =
            new() { Name = "Default Place", Description = "Default place, please ignore!" };

        Item firstTicket =
            new()
            {
                Name = "Default Ticket",
                Description = "Default ticket, please ignore!",
                Type = firstItemType,
                Amount = 1,
                Guid = Guid.NewGuid(),
                Place = firstPlace,
                StoreDate = DateTime.UtcNow,
            };

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Seed the database
            await using var transaction = await dbContext.Database.BeginTransactionAsync(
                cancellationToken
            );
            var typeEntity = await dbContext.ItemTypes.AddAsync(firstItemType, cancellationToken);
            var placeEntity = await dbContext.Places.AddAsync(firstPlace, cancellationToken);

            firstTicket.Type = typeEntity.Entity;
            firstTicket.Place = placeEntity.Entity;

            await dbContext.Items.AddAsync(firstTicket, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });*/
    }
}
