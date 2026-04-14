using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace at_prueba_tecnica_backend.Infrastructure.Persistence;

/// <summary>
/// Design-Time Factory para AppDbContext.
/// Permite que 'dotnet ef' funcione sin levantar la aplicación completa.
/// Se usa automáticamente durante add-migration y database-update.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();

        // Si estamos en Infrastructure, subir a la raíz
        if (basePath.EndsWith("at-prueba-tecnica-backend.Infrastructure"))
        {
            basePath = Path.GetDirectoryName(basePath) ?? basePath;
        }

        var apiPath = Path.Combine(basePath, "at-prueba-tecnica-backend.Api");

        var config = new ConfigurationBuilder()
            .SetBasePath(apiPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found in appsettings.json");

        optionsBuilder.UseSqlServer(
            connectionString,
            sql => sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));

        return new AppDbContext(optionsBuilder.Options);
    }
}
