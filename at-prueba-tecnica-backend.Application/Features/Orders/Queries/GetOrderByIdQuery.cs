using at_prueba_tecnica_backend.Application.Features.DTOs;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Orders.Queries;

/// <summary>
/// Query to retrieve a specific order by ID.
/// </summary>
public record GetOrderByIdQuery(Guid Id) : IRequest<Result<OrderDto>>;
