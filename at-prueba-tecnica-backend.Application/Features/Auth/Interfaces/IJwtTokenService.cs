using at_prueba_tecnica_backend.Domain.Entities;

namespace at_prueba_tecnica_backend.Application.Features.Auth.Interfaces;

/// <summary>
/// Contrato para servicio de generación y validación de JWT tokens.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Genera un JWT token para un usuario autenticado.
    /// </summary>
    string GenerateToken(User user);

    /// <summary>
    /// Valida un JWT token.
    /// </summary>
    bool ValidateToken(string token);
}
