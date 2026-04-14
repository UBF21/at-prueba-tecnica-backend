using at_prueba_tecnica_backend.Application.Features.DTOs;
using at_prueba_tecnica_backend.Domain.Entities;

namespace at_prueba_tecnica_backend.Application.Features.Mappings;

/// <summary>
/// Mapping extensions for Order entity.
/// </summary>
public static class OrderMappingExtensions
{
    /// <summary>
    /// Maps an Order entity to its DTO.
    /// </summary>
    public static OrderDto ToDto(this Order order, bool includeItems = false)
    {
        return new OrderDto(
            Code: order.Code,
            OrderNumber: order.OrderNumber,
            Total: order.Total,
            Status: order.Status.ToString(),
            CustomerId: order.CustomerId,
            CreatedAt: order.CreatedAt,
            UpdatedAt: order.UpdatedAt,
            DeletedAt: order.DeletedAt,
            Items: includeItems
                ? order.Items.Where(i => i.DeletedAt == null).Select(i => i.ToDto()).ToList()
                : null
        );
    }
}
