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
        
        modelBuilder.Entity<Model>()
            .HasOne(m => m.Make)
            .WithMany(m => m.PossibleModels);

        modelBuilder.Entity<Engine>()
            .HasOne(m => m.Make)
            .WithMany(m => m.PossibleEngines);

        modelBuilder.Entity<Engine>()
            .HasOne(m => m.Fuel)
            .WithMany(m => m.PossibleEngines);

        modelBuilder.Entity<Car>()
            .HasOne(m => m.Model)
            .WithMany(m => m.PossibleCars);

        modelBuilder.Entity<Car>()
            .HasMany(m => m.PossibleCategories)
            .WithMany(m => m.PossibleCars);
        
        modelBuilder.Entity<Car>()
            .HasMany(m => m.PossibleDoorTypes)
            .WithMany(m => m.PossibleCars);
        
        modelBuilder.Entity<Car>()
            .HasMany(m => m.PossibleTransmissions)
            .WithMany(m => m.PossibleCars);
        
        modelBuilder.Entity<Car>()
            .HasMany(m => m.PossibleTractions)
            .WithMany(m => m.PossibleCars);
        
        modelBuilder.Entity<Car>()
            .HasMany(m => m.PossibleEngines)
            .WithMany(m => m.PossibleCars);

        modelBuilder.Entity<Listing>()
            .HasOne(m => m.Car)
            .WithMany(m => m.Listings);

        modelBuilder.Entity<Listing>()
            .HasOne(m => m.Category)
            .WithMany(m => m.Listings);

        modelBuilder.Entity<Listing>()
            .HasOne(m => m.Engine)
            .WithMany(m => m.Listings);

        modelBuilder.Entity<Listing>()
            .HasOne(m => m.DoorType)
            .WithMany(m => m.Listings);

        modelBuilder.Entity<Listing>()
            .HasOne(m => m.Transmission)
            .WithMany(m => m.Listings);

        modelBuilder.Entity<Listing>()
            .HasOne(m => m.Traction)
            .WithMany(m => m.Listings);

        modelBuilder.Entity<Listing>()
            .HasOne(m => m.InteriorColor)
            .WithMany(c => c.ListingsInterior);
        
        modelBuilder.Entity<Listing>()
            .HasOne(m => m.ExteriorColor)
            .WithMany(m => m.ListingsExterior);

        modelBuilder.Entity<Listing>()
            .HasMany(m => m.FeaturesExterior)
            .WithMany(m => m.Listings);

        modelBuilder.Entity<Listing>()
            .HasMany(m => m.FeaturesInterior)
            .WithMany(m => m.Listings);
    }
}