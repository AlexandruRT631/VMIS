using Microsoft.EntityFrameworkCore;
using user_backend.Entities;

namespace user_backend.DataAccess;

public class UserDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }
    public DbSet<FavouriteListing> FavouriteListings { get; init; }
    public DbSet<FavouriteUser> FavouriteUsers { get; init; }
    public DbSet<Conversation> Conversations { get; init; }
    public DbSet<Message> Messages { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.FavouriteListings)
            .WithOne(fl => fl.User);

        modelBuilder.Entity<User>()
            .HasMany(u => u.FavouriteUsers)
            .WithOne(fu => fu.User);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Conversations)
            .WithMany(c => c.Users);

        modelBuilder.Entity<Conversation>()
            .HasMany(c => c.Messages)
            .WithOne(m => m.Conversation);
    }
}