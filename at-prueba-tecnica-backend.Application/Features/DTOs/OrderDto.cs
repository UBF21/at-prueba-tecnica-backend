namespace at_prueba_tecnica_backend.Application.Features.DTOs;

/// <summary>
/// Data Transfer Object for Order.
/// Exposes Code (int) as the public identifier.
/// </summary>
public record OrderDto(
    int Code,
    string OrderNumber,
    decimal Total,
    string Status,
    Guid CustomerId,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? DeletedAt,
    IReadOnlyList<OrderItemDto>? Items = null
);
