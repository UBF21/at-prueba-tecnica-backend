namespace at_prueba_tecnica_backend.Application.Features.DTOs;

/// <summary>
/// Data Transfer Object for OrderItem.
/// Represents a line item in an order.
/// </summary>
public record OrderItemDto(
    int Code,
    Guid OrderId,
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    decimal Subtotal,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? DeletedAt
);
