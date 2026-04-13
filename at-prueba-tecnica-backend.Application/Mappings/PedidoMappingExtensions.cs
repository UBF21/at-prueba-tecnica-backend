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
            Id: pedido.Id,
            NumeroPedido: pedido.NumeroPedido,
            Total: pedido.Total,
            Estado: pedido.Estado.ToString(),
            ClienteId: pedido.ClienteId,
            FechaCreacion: pedido.FechaCreacion,
            FechaModificacion: pedido.FechaModificacion
        );
    }
}
