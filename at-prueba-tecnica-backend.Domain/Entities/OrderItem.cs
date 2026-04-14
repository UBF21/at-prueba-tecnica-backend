namespace at_prueba_tecnica_backend.Domain.Entities;

/// <summary>
/// OrderItem entity. Represents a line item in an order.
///
/// Business rules:
/// - UnitPrice is a snapshot of the product price at the time of adding the item (immutable)
/// - Quantity must be > 0
/// - Subtotal is computed: Quantity * UnitPrice
/// - Deleted items do not appear in queries (soft delete)
/// </summary>
public class OrderItem : AuditableEntity
{
    /// <summary>Order ID (UUID) that this item belongs to (required, foreign key).</summary>
    public Guid OrderId { get; set; }

    /// <summary>Product ID (UUID) that this item references (required, foreign key).</summary>
    public Guid ProductId { get; set; }

    /// <summary>Quantity ordered (required, must be > 0).</summary>
    public int Quantity { get; set; }

    /// <summary>Unit price snapshot at the time of adding the item (required). This is immutable and does not change if the product price changes.</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>Computed subtotal = Quantity * UnitPrice.</summary>
    public decimal Subtotal => Quantity * UnitPrice;

    /// <summary>Navigation property to the Order.</summary>
    public Order? Order { get; set; }

    /// <summary>Navigation property to the Product.</summary>
    public Product? Product { get; set; }

    /// <summary>
    /// Marks the order item as deleted (soft delete).
    /// </summary>
    public void Delete()
    {
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Restores a deleted order item.
    /// </summary>
    public void Restore()
    {
        DeletedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }
}
