using at_prueba_tecnica_backend.Application.DTOs;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Pedidos.Commands;

/// <summary>
/// Command para crear un nuevo pedido.
/// </summary>
public record CreatePedidoCommand(string NumeroPedido, decimal Total, int ClienteId)
    : IRequest<Result<PedidoDto>>;
