using at_prueba_tecnica_backend.Application.DTOs;
using at_prueba_tecnica_backend.Application.Mappings;
using at_prueba_tecnica_backend.Application.Pedidos.Commands;
using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Enums;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Pedidos.Handlers;

/// <summary>
/// Handler para el command CreatePedidoCommand.
/// Crea un nuevo pedido tras validar la unicidad del número de pedido.
/// </summary>
public class CreatePedidoCommandHandler : IRequestHandler<CreatePedidoCommand, Result<PedidoDto>>
{
    private readonly IPedidoRepository _repo;

    public CreatePedidoCommandHandler(IPedidoRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<PedidoDto>> Handle(CreatePedidoCommand command, CancellationToken ct)
    {
        try
        {
            // Validar unicidad de NumeroPedido
            var existe = await _repo.ExistsByNumeroPedidoAsync(command.NumeroPedido, ct);

            if (existe)
                return Result<PedidoDto>.Fail(
                    "El número de pedido ya existe",
                    ErrorType.Conflict);

            var pedido = new Pedido
            {
                NumeroPedido = command.NumeroPedido,
                Total = command.Total,
                ClienteId = command.ClienteId,
                Estado = EstadoPedido.Pendiente,
                FechaCreacion = DateTime.UtcNow
            };

            await _repo.AddAsync(pedido, ct);
            return Result<PedidoDto>.Ok(pedido.ToDto());
        }
        catch (Exception ex)
        {
            return Result<PedidoDto>.Fail($"Error al crear pedido: {ex.Message}", ErrorType.Failure);
        }
    }
}
