using System.Diagnostics;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using OpenTelemetry.Trace;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.ExcelImporter;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Test.Faker;

namespace TuDa.CIMS.MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    IConfiguration configuration,
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
            if (!configuration.GetValue<bool>("Seeding"))
                return;
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
            }
        });
    }

    private static async Task RunMigrationAsync(
        CIMSDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }

    private async Task SeedDataAsync(CIMSDbContext dbContext, CancellationToken cancellationToken)
    {
        Randomizer.Seed = new Random(12345);

        (Consumable consumable, Chemical chemical) = await SeedAssetItem(
            dbContext,
            cancellationToken
        );

        var professor = new PersonFaker<Professor>().Generate();
        var students = new PersonFaker<Student>().GenerateBetween(5, 5);

        var purchaseEntries = new PurchaseEntryFaker(chemical).GenerateBetween(4, 4);
        var purchase = new PurchaseFaker(null, purchaseEntries).Generate();

        var workingGroup = new WorkingGroupFaker(professor, students, [purchase]).Generate();

        var consumableTransaction = new ConsumableTransactionFaker(consumable).Generate();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Seed the database
            await using var transaction = await dbContext.Database.BeginTransactionAsync(
                cancellationToken
            );

            if (!await dbContext.WorkingGroups.AnyAsync(cancellationToken))
            {
                await dbContext.WorkingGroups.AddAsync(workingGroup, cancellationToken);
                logger.LogInformation(
                    "Seeding Consumables with Id {WorkingGroupId}",
                    workingGroup.Id
                );
            }

            if (!await dbContext.ConsumableTransactions.AnyAsync(cancellationToken))
            {
                await dbContext.ConsumableTransactions.AddAsync(
                    consumableTransaction,
                    cancellationToken
                );
                logger.LogInformation(
                    "Seeding Consumables with Id {ConsumableTransactionId}",
                    consumableTransaction.Id
                );
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }

    private async Task<(Consumable, Chemical)> SeedAssetItem(
        CIMSDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var room = new RoomFaker().Generate();

        Chemical chemical;
        Consumable consumable;
        try
        {
            var reader = new AssetItemsExcelReader("./AssetItems.xlsx");
            var assetItems = reader.ReadAssetItems().ToList();

            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                if (!await dbContext.AssetItems.AnyAsync(cancellationToken))
                {
                    await dbContext.AssetItems.AddRangeAsync(assetItems, cancellationToken);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });

            chemical = assetItems.OfType<Chemical>().First();
            consumable = assetItems.OfType<Consumable>().First();
        }
        catch
        {
            logger.LogWarning("No excel file found in path './AssetItems.xlsx'");
            logger.LogWarning("Generating fake data");

            chemical = new ChemicalFaker(room).Generate();
            Solvent solvent = new SolventFaker().Generate();
            GasCylinder gasCylinder = new GasCylinderFaker().Generate();
            consumable = new ConsumableFaker(room).Generate();
            if (!await dbContext.Rooms.AnyAsync(cancellationToken))
            {
                await dbContext.Rooms.AddAsync(room, cancellationToken);
                logger.LogInformation("Seeding Room with Id {RoomId}", room.Id);
            }

            if (
                !await dbContext.Chemicals.Where(ch => !(ch is Solvent)).AnyAsync(cancellationToken)
            )
            {
                await dbContext.Chemicals.AddAsync(chemical, cancellationToken);
                logger.LogInformation("Seeding Chemicals with Id {ChemicalsId}", chemical.Id);
            }

            if (!await dbContext.Solvents.AnyAsync(cancellationToken))
            {
                await dbContext.Solvents.AddAsync(solvent, cancellationToken);
                logger.LogInformation("Seeding Solvents with Id {SolventId}", solvent.Id);
            }

            if (!await dbContext.GasCylinders.AnyAsync(cancellationToken))
            {
                await dbContext.GasCylinders.AddAsync(gasCylinder, cancellationToken);
                logger.LogInformation("Seeding Chemicals with Id {GasCylinderId}", gasCylinder.Id);
            }

            if (!await dbContext.Consumables.AnyAsync(cancellationToken))
            {
                await dbContext.Consumables.AddAsync(consumable, cancellationToken);
                logger.LogInformation("Seeding Consumables with Id {ConsumablesId}", consumable.Id);
            }
        }

        return (consumable, chemical);
    }
}
