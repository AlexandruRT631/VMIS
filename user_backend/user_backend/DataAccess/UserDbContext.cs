using Microsoft.EntityFrameworkCore;
using user_backend.Entities;

namespace user_backend.DataAccess;

public class UserDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }
}