namespace at_prueba_tecnica_backend.Domain.Entities;

/// <summary>
/// Clase base para todas las entidades que requieren auditoría.
/// Proporciona: Id (PK), Code (GUID único para compartir entre ambientes),
/// timestamps (CreatedAt, UpdatedAt, DeletedAt para soft delete).
/// </summary>
public abstract class AuditableEntity
{
    /// <summary>Identificador único interno (PK autoincremental).</summary>
    public int Id { get; set; }

    /// <summary>Código único (GUID) compartible entre ambientes - NO se expone el Id.</summary>
    public string Code { get; set; } = Guid.NewGuid().ToString();

    /// <summary>Fecha de creación en UTC.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Fecha de última actualización en UTC.</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Fecha de eliminación lógica (soft delete). NULL = no eliminado.</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Indica si la entidad fue eliminada lógicamente.</summary>
    public bool IsDeleted => DeletedAt.HasValue;
}
