using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Database;

public class CIMSDbContext(DbContextOptions<CIMSDbContext> options) : DbContext(options)
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
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Person> Persons { get; set; }

    #endregion

    #region Purchase

    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<PurchaseEntry> PurchaseEntries { get; set; }

    #endregion

    public DbSet<Hazard> Hazards { get; init; }
    public DbSet<ConsumableTransaction> ConsumableTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Purchase>()
            .HasMany(p => p.Entries)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<WorkingGroup>()
            .HasMany(p => p.Purchases)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder
            .Entity<WorkingGroup>()
            .HasMany(p => p.Students)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder
            .Entity<WorkingGroup>()
            .HasOne(p => p.Professor)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

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

        modelBuilder
            .Entity<Purchase>()
            .HasOne(p => p.Successor)
            .WithOne(p => p.Predecessor)
            .OnDelete(DeleteBehavior.NoAction);
    }

    public override int SaveChanges()
    {
        SetUpdatedAtTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetUpdatedAtTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void SetUpdatedAtTimestamps()
    {
        var entries = ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}
