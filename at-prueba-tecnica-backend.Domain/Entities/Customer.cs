namespace at_prueba_tecnica_backend.Domain.Entities;

/// <summary>
/// Customer entity. Represents a customer who can place orders.
///
/// Business rules:
/// - Email must be unique
/// - Deleted customers do not appear in queries (soft delete)
/// </summary>
public class Customer : AuditableEntity
{
    /// <summary>Customer name (required).</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Customer email address (required, unique).</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Customer phone number (optional).</summary>
    public string? Phone { get; set; }

    /// <summary>Customer address (optional).</summary>
    public string? Address { get; set; }

    /// <summary>Collection of orders placed by this customer.</summary>
    public ICollection<Order> Orders { get; set; } = new List<Order>();

    /// <summary>
    /// Marks the customer as deleted (soft delete).
    /// </summary>
    public void Delete()
    {
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Restores a deleted customer.
    /// </summary>
    public void Restore()
    {
        DeletedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }
}
