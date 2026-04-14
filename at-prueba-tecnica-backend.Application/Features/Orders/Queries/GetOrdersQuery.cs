using at_prueba_tecnica_backend.Application.Features.DTOs;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Orders.Queries;

/// <summary>
/// Query to retrieve a list of orders with pagination and optional status filtering.
/// Returns tuple with (orders, totalCount) to support pagination metadata.
/// </summary>
public record GetOrdersQuery(int Page = 1, int PageSize = 10, string? Status = null)
    : IRequest<Result<(List<OrderDto> Data, int Total)>>;
