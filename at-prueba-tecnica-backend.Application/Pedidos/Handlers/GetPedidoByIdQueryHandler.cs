using at_prueba_tecnica_backend.Application.DTOs;
using at_prueba_tecnica_backend.Application.Mappings;
using at_prueba_tecnica_backend.Application.Pedidos.Queries;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Pedidos.Handlers;

/// <summary>
/// Handler para la query GetPedidoByIdQuery.
/// Obtiene un pedido específico por su ID.
/// </summary>
public class GetPedidoByIdQueryHandler : IRequestHandler<GetPedidoByIdQuery, Result<PedidoDto>>
{
    private readonly IPedidoRepository _repo;

    public GetPedidoByIdQueryHandler(IPedidoRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<PedidoDto>> Handle(GetPedidoByIdQuery query, CancellationToken ct)
    {
        try
        {
            var pedido = await _repo.GetByIdAsync(query.Id, ct);

            if (pedido is null)
                return Result<PedidoDto>.Fail("Pedido no encontrado", ErrorType.NotFound);

            return Result<PedidoDto>.Ok(pedido.ToDto());
        }
        catch (Exception ex)
        {
            return Result<PedidoDto>.Fail($"Error al obtener pedido: {ex.Message}", ErrorType.Failure);
        }
    }
}
