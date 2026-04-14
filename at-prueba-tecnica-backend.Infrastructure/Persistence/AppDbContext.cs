using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace at_prueba_tecnica_backend.Infrastructure.Persistence;

/// <summary>
/// Application DbContext.
/// Configures all entities, relationships, indexes, and global filters (soft delete).
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Seed: admin user for development
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Code = "550e8400-e29b-41d4-a716-446655440000", // Fixed GUID for development
            Name = "Administrator",
            Email = "admin@retopedidos.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            Role = Role.Admin,
            CreatedAt = DateTime.UtcNow
        });
    }
}
