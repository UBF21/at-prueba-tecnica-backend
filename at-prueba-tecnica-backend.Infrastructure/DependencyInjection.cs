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
/// IServiceCollection extension to register Infrastructure services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers all Infrastructure services in the DI container.
    /// Includes DbContext, Repositories, JWT, and authentication configuration.
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext with SQL Server
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
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        // JWT Configuration
        var jwtSettings = new JwtSettings();
        configuration.GetSection("JwtSettings").Bind(jwtSettings);
        services.AddSingleton(jwtSettings);

        // JWT Service
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
