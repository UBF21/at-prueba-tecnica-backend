using at_prueba_tecnica_backend.Application.Features.DTOs;
using at_prueba_tecnica_backend.Domain.Entities;

namespace at_prueba_tecnica_backend.Application.Features.Mappings;

/// <summary>
/// Mapping extensions for OrderItem entity.
/// </summary>
public static class OrderItemMappingExtensions
{
    /// <summary>
    /// Maps an OrderItem entity to its DTO.
    /// </summary>
    public static OrderItemDto ToDto(this OrderItem item)
    {
        return new OrderItemDto(
            Code: item.Code,
            OrderId: item.OrderId,
            ProductId: item.ProductId,
            Quantity: item.Quantity,
            UnitPrice: item.UnitPrice,
            Subtotal: item.Subtotal,
            CreatedAt: item.CreatedAt,
            UpdatedAt: item.UpdatedAt,
            DeletedAt: item.DeletedAt
        );
    }
}
