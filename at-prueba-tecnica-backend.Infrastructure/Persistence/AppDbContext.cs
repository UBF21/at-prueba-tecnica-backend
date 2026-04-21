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
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Seed: admin user for development
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = new Guid("550e8400-e29b-41d4-a716-446655440000"),
                Code = 1,
                Name = "Administrator",
                Email = "admin@retopedidos.com",
                PasswordHash = "$2a$11$xiggvcJ1Tfe.BU9otSp11uN6fYwMLRKTjFczQ6YwXrefuDZYPuUVe",
                Role = Role.Admin,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = new Guid("550e8400-e29b-41d4-a716-446655440001"),
                Code = 2,
                Name = "User",
                Email = "user@retopedidos.com",
                PasswordHash = "$2a$11$YOeN8Xrq4dODGu.vEJ6qheBASX91rNqmlxmG7/D2hM3hZYkqFVDr2",
                Role = Role.User,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
