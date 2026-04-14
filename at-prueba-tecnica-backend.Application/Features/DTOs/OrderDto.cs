namespace at_prueba_tecnica_backend.Application.Features.DTOs;

/// <summary>
/// Data Transfer Object for Order.
/// Exposes Code as the shareable identifier across environments.
/// </summary>
public record OrderDto(
    string Code,
    string OrderNumber,
    decimal Total,
    string Status,
    int CustomerId,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? DeletedAt,
    IReadOnlyList<OrderItemDto>? Items = null
);
