namespace at_prueba_tecnica_backend.Infrastructure.Auth;

/// <summary>
/// Configuración de JWT tokens.
/// Se carga desde appsettings.json en la sección "JwtSettings".
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Clave secreta para firmar los tokens (debe tener al menos 32 caracteres).
    /// </summary>
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// Issuer (emisor) del token.
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Audience (público) del token.
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Tiempo de expiración del token en minutos.
    /// </summary>
    public int ExpiresInMinutes { get; set; } = 60;
}
