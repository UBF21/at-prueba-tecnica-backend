using at_prueba_tecnica_backend.Application.DTOs;
using at_prueba_tecnica_backend.Domain.Enums;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Pedidos.Commands;

/// <summary>
/// Command para actualizar un pedido existente.
/// </summary>
public record UpdatePedidoCommand(int Id, decimal Total, EstadoPedido Estado)
    : IRequest<Result<PedidoDto>>;
