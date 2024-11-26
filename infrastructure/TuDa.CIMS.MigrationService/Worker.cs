using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using OpenTelemetry.Trace;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Shared.Entities;

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
            var dbContext = scope.ServiceProvider.GetRequiredService<CIMSDbContext>();

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

    private async Task EnsureDatabaseAsync(
        CIMSDbContext dbContext,
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
                logger.LogInformation("Created Database \"CIMS\"");
            }
        });
    }

    private async Task RunMigrationAsync(
        CIMSDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
            logger.LogInformation("Migrated database");
        });
    }

    private async Task SeedDataAsync(CIMSDbContext dbContext, CancellationToken cancellationToken)
    {
        var hazard = new Hazard { Id = Guid.NewGuid(), Name = "Hazard" };
        var room = new Room { Id = Guid.NewGuid(), Name = "Haupt" };

        var chemical = new Chemical
        {
            Cas = "12-33-34232",
            Unit = "Liter",
            Id = Guid.NewGuid(),
            Room = room,
            Name = "Chemikalie",
            ItemNumber = "12345",
            Shop = "Chemikalien Shop",
            Hazards = [hazard],
            Note = "Notiz",
        };

        var consumable = new Consumable
        {
            Manufacturer = "Glasmacher",
            SerialNumber = "12343842738437293",
            Id = Guid.NewGuid(),
            Room = room,
            Name = "Verbrauchsgegenstand",
            ItemNumber = "12345",
            Shop = "Glas Shop",
            Note = "Notiz",
        };

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Seed the database
            await using var transaction = await dbContext.Database.BeginTransactionAsync(
                cancellationToken
            );

            if (!await dbContext.Hazards.AnyAsync(cancellationToken))
            {
                await dbContext.Hazards.AddAsync(hazard, cancellationToken);
                logger.LogInformation("Seeding Hazard with Id {HazardId}", hazard.Id);
            }

            if (!await dbContext.Rooms.AnyAsync(cancellationToken))
            {
                await dbContext.Rooms.AddAsync(room, cancellationToken);
                logger.LogInformation("Seeding Room with Id {RoomId}", room.Id);
            }

            if (!await dbContext.Chemicals.AnyAsync(cancellationToken))
            {
                await dbContext.Chemicals.AddAsync(chemical, cancellationToken);
                logger.LogInformation("Seeding Chemicals with Id {ChemicalsId}", chemical.Id);
            }

            if (!await dbContext.Consumables.AnyAsync(cancellationToken))
            {
                await dbContext.Consumables.AddAsync(consumable, cancellationToken);
                logger.LogInformation("Seeding Consumables with Id {ConsumablesId}", consumable.Id);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
}
