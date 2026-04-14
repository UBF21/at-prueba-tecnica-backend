using at_prueba_tecnica_backend.Application.DTOs;
using at_prueba_tecnica_backend.Application.Mappings;
using at_prueba_tecnica_backend.Application.Pedidos.Commands;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Pedidos.Handlers;

/// <summary>
/// Handler para el command UpdatePedidoCommand.
/// Actualiza un pedido existente con los nuevos valores.
/// </summary>
public class UpdatePedidoCommandHandler : IRequestHandler<UpdatePedidoCommand, Result<PedidoDto>>
{
    private readonly IPedidoRepository _repo;

    public UpdatePedidoCommandHandler(IPedidoRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<PedidoDto>> Handle(UpdatePedidoCommand command, CancellationToken ct)
    {
        try
        {
            var pedido = await _repo.GetByIdAsync(command.Id, ct);

            if (pedido is null)
                return Result<PedidoDto>.Fail("Pedido no encontrado", ErrorType.NotFound);

            pedido.Total = command.Total;
            pedido.Estado = command.Estado;
            pedido.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(pedido, ct);
            return Result<PedidoDto>.Ok(pedido.ToDto());
        }
        catch (Exception ex)
        {
            return Result<PedidoDto>.Fail($"Error al actualizar pedido: {ex.Message}", ErrorType.Failure);
        }
    }
}
