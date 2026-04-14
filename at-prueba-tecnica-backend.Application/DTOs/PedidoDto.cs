namespace at_prueba_tecnica_backend.Application.DTOs;

/// <summary>
/// DTO de Pedido para transferencia de datos entre capas.
/// Code es el identificador compartible entre ambientes.
/// </summary>
public record PedidoDto(
    string Code,
    string NumeroPedido,
    decimal Total,
    string Estado,
    int ClienteId,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? DeletedAt
);