using System.Text;
using at_prueba_tecnica_backend.Api.Middlewares;
using at_prueba_tecnica_backend.Application;
using at_prueba_tecnica_backend.Infrastructure;
using at_prueba_tecnica_backend.Infrastructure.Auth;
using at_prueba_tecnica_backend.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Vali_Mediator.Core.General.Extension;
using Vali_Validation.Core.Extensions;
using Vali_Validation.ValiMediator;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure DI (must be before validators since validators depend on repositories)
builder.Services.AddInfrastructure(builder.Configuration);

// Register Vali-Validation validators from assembly
builder.Services.AddValidationsFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);

// Vali-Mediator con Vali-Validation integration
builder.Services.AddValiMediator(config =>
{
    config.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
    config.AddValiValidationBehavior();
});

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
    ?? throw new InvalidOperationException("JwtSettings not configured in appsettings.json");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Authorization
builder.Services.AddAuthorization();

// OpenAPI (requerido por Scalar)
builder.Services.AddOpenApi();

// Controllers with camelCase JSON serialization
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// CORS para frontend
builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
        ?? new[] { "http://localhost:5173", "http://localhost:5174" };

    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Global exception middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

// HTTP pipeline
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

// OpenAPI y Scalar documentation (ANTES de autenticación para acceso público)
app.MapOpenApi();
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
}

app.UseAuthentication();
app.UseAuthorization();

// Controllers
app.MapControllers();

// Esperar a que SQL Server esté listo e inicializar BD
await Task.Delay(5000); // Esperar 5 segundos a que SQL Server esté completamente listo

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Reintentar hasta 10 veces con delays crecientes
    int maxRetries = 10;
    for (int attempt = 0; attempt < maxRetries; attempt++)
    {
        try
        {
            Console.WriteLine($"[Intento {attempt + 1}/{maxRetries}] Intentando conectar a SQL Server...");

            // Crear BD y tablas si no existen
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();
            Console.WriteLine("✅ BD creada e inicializada correctamente");
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️  Error en intento {attempt + 1}: {ex.Message}");

            if (attempt == maxRetries - 1)
            {
                Console.WriteLine($"❌ Error definitivo inicializando BD después de {maxRetries} intentos");
                Console.WriteLine($"Detalles: {ex}");
                throw;
            }

            // Esperar antes de reintentar (backoff exponencial)
            int delayMs = 1000 * (attempt + 1);
            Console.WriteLine($"Esperando {delayMs}ms antes de reintentar...");
            await Task.Delay(delayMs);
        }
    }
}

app.Run();