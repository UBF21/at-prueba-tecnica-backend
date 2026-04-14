namespace at_prueba_tecnica_backend.Domain.Enums;

/// <summary>
/// Order status enumeration representing the lifecycle of an order.
/// </summary>
public enum OrderStatus
{
    /// <summary>Order is pending confirmation.</summary>
    Pending = 1,

    /// <summary>Order has been confirmed and is being processed.</summary>
    Processing = 2,

    /// <summary>Order has been shipped.</summary>
    Shipped = 3,

    /// <summary>Order has been delivered to the customer.</summary>
    Delivered = 4,

    /// <summary>Order has been cancelled.</summary>
    Cancelled = 5
}
