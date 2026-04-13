using System.Text;
using at_prueba_tecnica_backend.Api.Middlewares;
using at_prueba_tecnica_backend.Application;
using at_prueba_tecnica_backend.Infrastructure;
using at_prueba_tecnica_backend.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Vali_Validation.ValiMediator;

var builder = WebApplication.CreateBuilder(args);

// Vali-Mediator con Vali-Validation integration
builder.Services.AddValiMediatorWithValidation(config =>
{
    config.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
}, typeof(ApplicationAssemblyMarker).Assembly);

// Infrastructure DI
builder.Services.AddInfrastructure(builder.Configuration);

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

// Scalar API documentation
builder.Services.AddScalar(options =>
{
    options.Title = "RetoPedidos API";
    options.DefaultHttpClient = new("http://localhost:5000", "http://localhost:5000");
});

// Controllers
builder.Services.AddControllers();

// CORS para frontend en desarrollo
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Global exception middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

// HTTP pipeline
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

// Scalar documentation
app.MapScalarApiReference();
app.MapOpenApi();

// Controllers
app.MapControllers();

app.Run();
