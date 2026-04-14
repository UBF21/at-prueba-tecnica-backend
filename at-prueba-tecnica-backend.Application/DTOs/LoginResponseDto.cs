namespace at_prueba_tecnica_backend.Application.DTOs;

/// <summary>
/// DTO de respuesta para login exitoso.
/// Contiene el JWT token y datos del usuario autenticado.
/// </summary>
public record LoginResponseDto(string Token, string Email, string Nombre, string Rol);
