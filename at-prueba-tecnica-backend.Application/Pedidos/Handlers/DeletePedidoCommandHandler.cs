using at_prueba_tecnica_backend.Application.Pedidos.Commands;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Pedidos.Handlers;

/// <summary>
/// Handler para el command DeletePedidoCommand.
/// Realiza soft delete de un pedido marcándolo como eliminado.
/// </summary>
public class DeletePedidoCommandHandler : IRequestHandler<DeletePedidoCommand, Result<bool>>
{
    private readonly IPedidoRepository _repo;

    public DeletePedidoCommandHandler(IPedidoRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<bool>> Handle(DeletePedidoCommand command, CancellationToken ct)
    {
        try
        {
            var pedido = await _repo.GetByIdAsync(command.Id, ct);

            if (pedido is null)
                return Result<bool>.Fail("Pedido no encontrado", ErrorType.NotFound);

            pedido.Eliminar();
            await _repo.UpdateAsync(pedido, ct);

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Error al eliminar pedido: {ex.Message}", ErrorType.Failure);
        }
    }
}
