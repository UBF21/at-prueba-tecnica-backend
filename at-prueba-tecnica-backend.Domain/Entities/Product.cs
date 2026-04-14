namespace at_prueba_tecnica_backend.Domain.Entities;

/// <summary>
/// Product entity. Represents a product that can be ordered.
///
/// Business rules:
/// - UnitPrice must be greater than 0
/// - Stock must be >= 0
/// - Deleted products do not appear in queries (soft delete)
/// </summary>
public class Product : AuditableEntity
{
    /// <summary>Product name (required).</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Product description (optional).</summary>
    public string? Description { get; set; }

    /// <summary>Unit price of the product (required, must be > 0).</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>Available stock quantity (required).</summary>
    public int Stock { get; set; }

    /// <summary>Collection of order items that reference this product.</summary>
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    /// <summary>
    /// Marks the product as deleted (soft delete).
    /// </summary>
    public void Delete()
    {
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Restores a deleted product.
    /// </summary>
    public void Restore()
    {
        DeletedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }
}
