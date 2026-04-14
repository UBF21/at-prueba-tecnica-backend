using at_prueba_tecnica_backend.Application.Auth.Interfaces;
using at_prueba_tecnica_backend.Domain.Interfaces;
using at_prueba_tecnica_backend.Infrastructure.Auth;
using at_prueba_tecnica_backend.Infrastructure.Persistence;
using at_prueba_tecnica_backend.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace at_prueba_tecnica_backend.Infrastructure;

/// <summary>
/// Extensión de IServiceCollection para registrar servicios de Infrastructure.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra todos los servicios de Infrastructure en el contenedor DI.
    /// Incluye DbContext, Repositories, JWT y configuración de autenticación.
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext con SQL Server
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString, sql =>
            {
                sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                sql.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            }));

        // Repositories
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        // JWT Configuration
        var jwtSettings = new JwtSettings();
        configuration.GetSection("JwtSettings").Bind(jwtSettings);
        services.AddSingleton(jwtSettings);

        // JWT Service
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
