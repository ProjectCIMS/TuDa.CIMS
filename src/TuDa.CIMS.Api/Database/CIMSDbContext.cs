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

    public DbSet<Hazard> Hazards { get; init; }
    public DbSet<Room> Rooms { get; init; }
    public DbSet<ConsumableTransaction> ConsumableTransactions { get; set; }

    public CIMSDbContext(DbContextOptions<CIMSDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Substance>()
            .HasMany(s => s.Hazards)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "SubstanceHazard", // Join table name
                j =>
                    j.HasOne<Hazard>() // Hazard side of the join table
                        .WithMany()
                        .HasForeignKey("HazardId")
                        .OnDelete(DeleteBehavior.Cascade), // Configure deletion behavior
                j =>
                    j.HasOne<Substance>() // Substance side of the join table
                        .WithMany()
                        .HasForeignKey("SubstanceId")
                        .OnDelete(DeleteBehavior.Cascade), // Configure deletion behavior
                j =>
                {
                    j.HasKey("SubstanceId", "HazardId"); // Composite primary key
                }
            );

        modelBuilder.Entity<Professor>(entity =>
        {
            entity.OwnsOne(p => p.Address, address =>
            {
                address.Property(a => a.Street);
                address.Property(a => a.Number);
                address.Property(a => a.ZipCode);
                address.Property(a => a.City);

                address.ToTable("Professors");
            });
        });
    }
}
