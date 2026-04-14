using System.Linq;
using Vali_Flow.Core;
using Vali_Flow.Classes.Specification;
using at_prueba_tecnica_backend.Application.Features.DTOs;
using at_prueba_tecnica_backend.Application.Features.Mappings;
using at_prueba_tecnica_backend.Application.Features.Orders.Filters;
using at_prueba_tecnica_backend.Application.Features.Orders.Queries;
using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Vali_Flow.Abstractions;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Orders.Handlers;

/// <summary>
/// Handler for GetOrdersQuery.
/// Retrieves orders with pagination and optional status filtering using Vali-Flow specifications.
/// Returns tuple with (orders, totalCount) for pagination metadata.
/// </summary>
public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, Result<(List<OrderDto> Data, int Total)>>
{
    private readonly IOrderRepository _repo;

    public GetOrdersQueryHandler(IOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<(List<OrderDto> Data, int Total)>> Handle(GetOrdersQuery query, CancellationToken ct)
    {
        try
        {
            // Get total count without pagination
            var countSpec = new QuerySpecification<Order>()
                .WithFilter(OrderFilters.ActiveByStatus(query.Status));

            var countQueryable = await _repo.EvaluateQueryAsync(countSpec);
            var total = await countQueryable.CountAsync(ct);

            // Get paginated results
            var spec = new QuerySpecification<Order>()
                .WithFilter(OrderFilters.ActiveByStatus(query.Status))
                .WithOrderBy(o => o.CreatedAt, ascending: false)
                .WithPagination(query.Page, query.PageSize)
                .WithAsNoTracking(true);

            var queryable = await _repo.EvaluateQueryAsync(spec);
            var orders = await queryable.Include(o => o.Customer).ToListAsync(ct);

            var dtos = orders.Select(o => o.ToDto()).ToList();
            return Result<(List<OrderDto>, int)>.Ok((dtos, total));
        }
        catch (Exception ex)
        {
            return Result<(List<OrderDto>, int)>.Fail($"Error retrieving orders: {ex.Message}", ErrorType.Failure);
        }
    }
}
