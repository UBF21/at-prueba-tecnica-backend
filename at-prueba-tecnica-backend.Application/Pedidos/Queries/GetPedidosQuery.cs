using at_prueba_tecnica_backend.Application.DTOs;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Pedidos.Queries;

/// <summary>
/// Query para obtener la lista de pedidos con paginación y filtrado opcional.
/// </summary>
public record GetPedidosQuery(int Page = 1, int PageSize = 10, string? Estado = null)
    : IRequest<Result<List<PedidoDto>>>;
