using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using at_prueba_tecnica_backend.Application.Features.Auth.Interfaces;
using at_prueba_tecnica_backend.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace at_prueba_tecnica_backend.Infrastructure.Auth;

/// <summary>
/// Servicio de generación y validación de JWT tokens.
/// Usa configuración de appsettings.json para secret, issuer, audience y expiration.
/// </summary>
public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public string GenerateToken(User user)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
