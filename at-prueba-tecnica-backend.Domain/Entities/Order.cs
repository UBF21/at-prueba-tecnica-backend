using at_prueba_tecnica_backend.Domain.Enums;

namespace at_prueba_tecnica_backend.Domain.Entities;

/// <summary>
/// Order entity. Represents a customer order in the system.
///
/// Business rules:
/// - OrderNumber must be unique
/// - Total is calculated from OrderItems
/// - Deleted orders do not appear in queries (soft delete via DeletedAt)
/// </summary>
public class Order : AuditableEntity
{
    /// <summary>Unique order number (required). Example: "ORD-001", "ORD-2026-001".</summary>
    public string OrderNumber { get; set; } = string.Empty;

    /// <summary>Total amount of the order (computed from OrderItems).</summary>
    public decimal Total { get; set; }

    /// <summary>Current status of the order (required).</summary>
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    /// <summary>Customer ID (UUID) who placed the order (required).</summary>
    public Guid CustomerId { get; set; }

    /// <summary>Navigation property to the customer who placed the order.</summary>
    public Customer? Customer { get; set; }

    /// <summary>Collection of order line items.</summary>
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    /// <summary>
    /// Marks the order as deleted (soft delete).
    /// </summary>
    public void Delete()
    {
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Restores a deleted order.
    /// </summary>
    public void Restore()
    {
        DeletedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Recalculates the order total from order items.
    /// </summary>
    public void RecalculateTotal()
    {
        Total = Items.Sum(i => i.Subtotal);
    }
}
