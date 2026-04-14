using at_prueba_tecnica_backend.Application.DTOs;
using at_prueba_tecnica_backend.Application.Mappings;
using at_prueba_tecnica_backend.Application.Pedidos.Queries;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Pedidos.Handlers;

/// <summary>
/// Handler para la query GetPedidosQuery.
/// Obtiene pedidos con paginación y filtrado opcional por estado.
/// </summary>
public class GetPedidosQueryHandler : IRequestHandler<GetPedidosQuery, Result<List<PedidoDto>>>
{
    private readonly IPedidoRepository _repo;

    public GetPedidosQueryHandler(IPedidoRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<List<PedidoDto>>> Handle(GetPedidosQuery query, CancellationToken ct)
    {
        try
        {
            var pedidos = await _repo.GetAllAsync(ct);

            // Filtrar por estado si se proporciona
            if (!string.IsNullOrEmpty(query.Estado))
                pedidos = pedidos.Where(p => p.Estado.ToString() == query.Estado).ToList();

            // Ordenar por fecha de creación descendente
            var ordenados = pedidos.OrderByDescending(p => p.CreatedAt).ToList();

            // Aplicar paginación
            var paginados = ordenados
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            var dtos = paginados.Select(p => p.ToDto()).ToList();
            return Result<List<PedidoDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<List<PedidoDto>>.Fail($"Error al obtener pedidos: {ex.Message}", ErrorType.Failure);
        }
    }
}
