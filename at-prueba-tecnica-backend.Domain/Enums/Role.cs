namespace at_prueba_tecnica_backend.Domain.Enums;

/// <summary>
/// Role enumeration for user access control.
/// </summary>
public enum Role
{
    /// <summary>Administrator with full system access.</summary>
    Admin = 0,

    /// <summary>Regular user with standard permissions.</summary>
    User = 1,

    /// <summary>Viewer with read-only access.</summary>
    Viewer = 2
}
