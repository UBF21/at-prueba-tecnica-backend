using at_prueba_tecnica_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace at_prueba_tecnica_backend.Infrastructure.Persistence;

/// <summary>
/// DbContext principal de la aplicación.
/// Configura todas las entidades, relaciones, índices y global filters (soft delete).
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplicar todas las configuraciones de entidades desde este ensamblado
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Seed: Usuario administrador para el reto
        modelBuilder.Entity<Usuario>().HasData(new Usuario
        {
            Id = 1,
            Nombre = "Administrador",
            Email = "admin@retopedidos.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!")
        });
    }
}
