namespace at_prueba_tecnica_backend.Domain.Entities;

/// <summary>
/// Clase base para todas las entidades que requieren auditoría.
/// Proporciona: Id (UUID/GUID PK), Code (int secuencial para referencias públicas),
/// timestamps (CreatedAt, UpdatedAt, DeletedAt para soft delete).
/// </summary>
public abstract class AuditableEntity
{
    /// <summary>Identificador único global (PK UUID/GUID) - Seguro, no expone secuencia.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Código secuencial entero (autoincremental) para referencias públicas y legibilidad.</summary>
    public int Code { get; set; }

    /// <summary>Fecha de creación en UTC.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Fecha de última actualización en UTC.</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Fecha de eliminación lógica (soft delete). NULL = no eliminado.</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Indica si la entidad fue eliminada lógicamente.</summary>
    public bool IsDeleted => DeletedAt.HasValue;
}
