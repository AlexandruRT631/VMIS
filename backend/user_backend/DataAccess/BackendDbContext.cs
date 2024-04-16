using Microsoft.EntityFrameworkCore;
using user_backend.Entities;

namespace user_backend.DataAccess;

public class BackendDbContext : DbContext
{
    public BackendDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
}