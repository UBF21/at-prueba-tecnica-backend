using Vali_Flow.Core;
using at_prueba_tecnica_backend.Application.Features.DTOs;
using at_prueba_tecnica_backend.Application.Features.Mappings;
using at_prueba_tecnica_backend.Application.Features.Orders.Filters;
using at_prueba_tecnica_backend.Application.Features.Orders.Queries;
using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Flow.Abstractions;
using Vali_Flow.Classes.Specification;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Orders.Handlers;

/// <summary>
/// Handler for GetOrderByIdQuery.
/// Retrieves a specific order by ID using Vali-Flow BasicSpecification.
/// </summary>
public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>
{
    private readonly IOrderRepository _repo;

    public GetOrderByIdQueryHandler(IOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery query, CancellationToken ct)
    {
        try
        {
            var spec = new BasicSpecification<Order>()
                .WithFilter(OrderFilters.ById(query.Id))
                .WithAsNoTracking(true);

            var order = await _repo.EvaluateGetFirstAsync(spec, ct);

            if (order is null)
                return Result<OrderDto>.Fail("Order not found", ErrorType.NotFound);

            return Result<OrderDto>.Ok(order.ToDto());
        }
        catch (Exception ex)
        {
            return Result<OrderDto>.Fail($"Error retrieving order: {ex.Message}", ErrorType.Failure);
        }
    }
}
