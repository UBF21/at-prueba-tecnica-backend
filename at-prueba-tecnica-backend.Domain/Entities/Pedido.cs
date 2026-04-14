using at_prueba_tecnica_backend.Domain.Enums;

namespace at_prueba_tecnica_backend.Domain.Entities;

/// <summary>
/// Entidad de Pedido. Representa un pedido en el sistema.
///
/// Reglas de negocio:
/// - El número de pedido debe ser único.
/// - El total debe ser mayor a 0.
/// - Los pedidos eliminados no aparecen en consultas (soft delete).
/// </summary>
public class Pedido : AuditableEntity
{
    /// <summary>Número único del pedido (requerido). Ej: "PED-001", "PED-2026-001".</summary>
    public string NumeroPedido { get; set; } = string.Empty;

    /// <summary>Total del pedido en moneda decimal (requerido). Debe ser mayor a 0.</summary>
    public decimal Total { get; set; }

    /// <summary>Estado actual del pedido (requerido).</summary>
    public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;

    /// <summary>ID del cliente que realiza el pedido (requerido).</summary>
    public int ClienteId { get; set; }

    /// <summary>
    /// Marca el pedido como eliminado (soft delete).
    /// </summary>
    public void Eliminar()
    {
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Restaura un pedido eliminado.
    /// </summary>
    public void Restaurar()
    {
        DeletedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }
}
