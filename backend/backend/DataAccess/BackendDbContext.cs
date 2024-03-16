using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.DataAccess;

public class BackendDbContext : DbContext
{
    public BackendDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
}