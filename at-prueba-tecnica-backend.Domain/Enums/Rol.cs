namespace at_prueba_tecnica_backend.Domain.Enums;

/// <summary>
/// Roles de usuario en el sistema.
/// </summary>
public enum Rol
{
    /// <summary>Administrador - acceso total al sistema.</summary>
    Admin = 0,

    /// <summary>Usuario regular - acceso limitado.</summary>
    Usuario = 1,

    /// <summary>Visualizador - solo lectura.</summary>
    Visualizador = 2
}
