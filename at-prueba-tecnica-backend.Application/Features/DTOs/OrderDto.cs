namespace at_prueba_tecnica_backend.Application.Features.DTOs;

/// <summary>
/// Data Transfer Object for Order.
/// </summary>
public record OrderDto(
    Guid Id,
    int Code,
    string OrderNumber,
    decimal Total,
    string Status,
    Guid CustomerId,
    string? CustomerName,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? DeletedAt,
    IReadOnlyList<OrderItemDto>? Items = null
);
