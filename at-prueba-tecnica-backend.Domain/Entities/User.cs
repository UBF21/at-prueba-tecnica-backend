using at_prueba_tecnica_backend.Domain.Enums;

namespace at_prueba_tecnica_backend.Domain.Entities;

/// <summary>
/// User entity. Represents an authenticated user in the system.
/// Inherits from AuditableEntity to get: Id, Code, CreatedAt, UpdatedAt, DeletedAt.
/// </summary>
public class User : AuditableEntity
{
    /// <summary>Unique email address (required).</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Password hash using BCrypt (required).</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>User full name (required).</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>User role (Admin, User, Viewer).</summary>
    public Role Role { get; set; } = Role.User;

    /// <summary>
    /// Marks the user as deleted (soft delete).
    /// </summary>
    public void Delete()
    {
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Restores a deleted user.
    /// </summary>
    public void Restore()
    {
        DeletedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }
}
