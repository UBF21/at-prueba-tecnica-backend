namespace at_prueba_tecnica_backend.Application.DTOs;

/// <summary>
/// DTO de Pedido para transferencia de datos entre capas.
/// </summary>
public record PedidoDto(
    int Id,
    string NumeroPedido,
    decimal Total,
    string Estado,
    int ClienteId,
    DateTime FechaCreacion,
    DateTime? FechaModificacion
);
