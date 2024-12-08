using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Database;

public class CIMSDbContext : DbContext
{
    public DbSet<AssetItem> AssetItems { get; init; }
    public DbSet<Consumable> Consumables { get; init; }
    public DbSet<Substance> Substances { get; init; }
    public DbSet<GasCylinder> GasCylinders { get; set; }
    public DbSet<Chemical> Chemicals { get; init; }
    public DbSet<Solvent> Solvents { get; set; }
    public DbSet<Hazard> Hazards { get; init; }
    public DbSet<Room> Rooms { get; init; }

    public CIMSDbContext(DbContextOptions<CIMSDbContext> options)
        : base(options) { }
}
