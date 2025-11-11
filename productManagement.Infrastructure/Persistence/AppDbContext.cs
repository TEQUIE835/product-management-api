using System.Reflection;
using Microsoft.EntityFrameworkCore;
using productManagement.Domain.Entities;

namespace productManagement.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext( DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}