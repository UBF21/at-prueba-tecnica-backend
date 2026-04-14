using at_prueba_tecnica_backend.Domain.Enums;

namespace at_prueba_tecnica_backend.Domain.Entities;

/// <summary>
/// Entidad de Usuario. Representa un usuario autenticado en el sistema.
/// Hereda de AuditableEntity para obtener: Id, Code, CreatedAt, UpdatedAt, DeletedAt.
/// </summary>
public class Usuario : AuditableEntity
{
    /// <summary>Email único del usuario (requerido).</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Hash de la contraseña usando BCrypt (requerido).</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>Nombre completo del usuario (requerido).</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Rol del usuario (Admin, Usuario, Visualizador).</summary>
    public Rol Rol { get; set; } = Rol.Usuario;
}
