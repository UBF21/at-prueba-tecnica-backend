namespace at_prueba_tecnica_backend.Domain.Entities;

/// <summary>
/// Entidad de Usuario. Representa un usuario autenticado en el sistema.
/// </summary>
public class Usuario
{
    /// <summary>Identificador único del usuario.</summary>
    public int Id { get; set; }

    /// <summary>Email único del usuario (requerido).</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Hash de la contraseña usando BCrypt (requerido).</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>Nombre completo del usuario (requerido).</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Fecha de creación del usuario.</summary>
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>Fecha de última modificación del usuario.</summary>
    public DateTime? FechaModificacion { get; set; }

    /// <summary>Soft delete: indica si el usuario fue eliminado lógicamente.</summary>
    public bool Eliminado { get; set; }
}
