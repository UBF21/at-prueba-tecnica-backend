using at_prueba_tecnica_backend.Application.DTOs;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Pedidos.Queries;

/// <summary>
/// Query para obtener un pedido específico por ID.
/// </summary>
public record GetPedidoByIdQuery(int Id) : IRequest<Result<PedidoDto>>;
