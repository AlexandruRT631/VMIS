using listing_backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace listing_backend.DataAccess;

public class ListingDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Category> Categories { get; init; }
    public DbSet<Color> Colors { get; init; }
    public DbSet<DoorType> DoorTypes { get; init; }
    public DbSet<Engine> Engines { get; init; }
    public DbSet<Fuel> Fuels { get; init; }
    public DbSet<Listing> Listings { get; init; }
    public DbSet<Make> Makes { get; init; }
    public DbSet<Model> Models { get; init; }
    public DbSet<Car> Cars { get; init; }
    public DbSet<Traction> Tractions { get; init; }
    public DbSet<Transmission> Transmissions { get; init; }
    public DbSet<FeatureInterior> FeaturesInterior { get; init; }
    public DbSet<FeatureExterior> FeaturesExterior { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Listing>()
            .HasOne(m => m.InteriorColor)
            .WithMany(c => c.ListingsInterior);
        
        modelBuilder.Entity<Listing>()
            .HasOne(m => m.ExteriorColor)
            .WithMany(m => m.ListingsExterior);

        modelBuilder.Entity<Make>()
            .HasMany(m => m.PossibleModels)
            .WithOne(m => m.Make);
    }
}