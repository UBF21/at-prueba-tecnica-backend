using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Pedidos.Commands;

/// <summary>
/// Command para eliminar un pedido (soft delete).
/// </summary>
public record DeletePedidoCommand(int Id) : IRequest<Result<bool>>;
