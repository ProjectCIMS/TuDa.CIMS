using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Database;

public class CIMSDbContext : DbContext
{
    #region AssetItem

    public DbSet<AssetItem> AssetItems { get; init; }
    public DbSet<Consumable> Consumables { get; init; }
    public DbSet<Substance> Substances { get; init; }
    public DbSet<GasCylinder> GasCylinders { get; set; }
    public DbSet<Chemical> Chemicals { get; init; }
    public DbSet<Solvent> Solvents { get; set; }

    #endregion

    #region WorkingGroup

    public DbSet<WorkingGroup> WorkingGroups { get; set; }
    public DbSet<Professor> Professors { get; set; }
    public DbSet<Student> Students { get; set; }

    #endregion

    #region Purchase

    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<PurchaseEntry> PurchaseEntries { get; set; }

    #endregion

    public DbSet<Person> Persons { get; set; }

    public DbSet<Hazard> Hazards { get; init; }
    public DbSet<Room> Rooms { get; init; }
    public DbSet<ConsumableTransaction> ConsumableTransactions { get; set; }

    public CIMSDbContext(DbContextOptions<CIMSDbContext> options)
        : base(options) { }
}
