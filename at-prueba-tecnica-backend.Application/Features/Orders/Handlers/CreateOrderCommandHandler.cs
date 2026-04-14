using Vali_Flow.Core;
using at_prueba_tecnica_backend.Application.Features.DTOs;
using at_prueba_tecnica_backend.Application.Features.Mappings;
using at_prueba_tecnica_backend.Application.Features.Orders.Commands;
using at_prueba_tecnica_backend.Application.Features.Orders.Filters;
using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Enums;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Flow.Abstractions;
using Vali_Flow.Classes.Specification;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Orders.Handlers;

/// <summary>
/// Handler for CreateOrderCommand.
/// Creates a new order with Pending status and validates order number uniqueness.
/// </summary>
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderDto>>
{
    private readonly IOrderRepository _repo;

    public CreateOrderCommandHandler(IOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<OrderDto>> Handle(CreateOrderCommand command, CancellationToken ct)
    {
        try
        {
            // Validate order number uniqueness
            var existsSpec = new BasicSpecification<Order>()
                .WithFilter(OrderFilters.ByOrderNumber(command.OrderNumber));

            var exists = await _repo.EvaluateAnyAsync(existsSpec, ct);
            if (exists)
                return Result<OrderDto>.Fail("Order number already exists", ErrorType.Conflict);

            var newOrder = new Order
            {
                OrderNumber = command.OrderNumber,
                CustomerId = command.CustomerId,
                Status = OrderStatus.Pending,
                Total = 0m,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(newOrder, saveChanges: true, ct);

            return Result<OrderDto>.Ok(newOrder.ToDto());
        }
        catch (Exception ex)
        {
            return Result<OrderDto>.Fail($"Error creating order: {ex.Message}", ErrorType.Failure);
        }
    }
}
