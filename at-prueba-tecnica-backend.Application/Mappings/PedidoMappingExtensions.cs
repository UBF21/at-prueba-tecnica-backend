using at_prueba_tecnica_backend.Application.DTOs;
using at_prueba_tecnica_backend.Domain.Entities;

namespace at_prueba_tecnica_backend.Application.Mappings;

/// <summary>
/// Extensiones de mapeo para Pedido.
/// </summary>
public static class PedidoMappingExtensions
{
    /// <summary>
    /// Mapea una entidad Pedido a su DTO.
    /// </summary>
    public static PedidoDto ToDto(this Pedido pedido)
    {
        return new PedidoDto(
            Code: pedido.Code,
            NumeroPedido: pedido.NumeroPedido,
            Total: pedido.Total,
            Estado: pedido.Estado.ToString(),
            ClienteId: pedido.ClienteId,
            CreatedAt: pedido.CreatedAt,
            UpdatedAt: pedido.UpdatedAt,
            DeletedAt: pedido.DeletedAt
        );
    }
}
