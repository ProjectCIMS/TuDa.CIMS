using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api;

using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Shared.Entities;

public class ApplicationDbContext : DbContext
{
    public DbSet<AssetItem> AssetItems { get; init; }
    public DbSet<Chemical> Chemicals { get; init; }
    public DbSet<Consumable> Consumables { get; init; }
    public DbSet<Hazard> Hazards { get; init; }
    public DbSet<Room> Rooms { get; init; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
}
