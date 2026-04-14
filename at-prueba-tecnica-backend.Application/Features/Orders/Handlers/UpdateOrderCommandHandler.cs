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
/// Handler for UpdateOrderCommand.
/// Updates order number and/or status with validation.
/// </summary>
public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Result<OrderDto>>
{
    private readonly IOrderRepository _repo;

    public UpdateOrderCommandHandler(IOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<OrderDto>> Handle(UpdateOrderCommand command, CancellationToken ct)
    {
        try
        {
            // Get order by ID
            var spec = new BasicSpecification<Order>()
                .WithFilter(OrderFilters.ById(command.Id));

            var order = await _repo.EvaluateGetFirstAsync(spec, ct);

            if (order is null)
                return Result<OrderDto>.Fail("Order not found", ErrorType.NotFound);

            // Update order number if provided
            if (!string.IsNullOrEmpty(command.OrderNumber) && order.OrderNumber != command.OrderNumber)
            {
                // Validate uniqueness of new order number
                var existsSpec = new BasicSpecification<Order>()
                    .WithFilter(OrderFilters.ByOrderNumber(command.OrderNumber));

                var exists = await _repo.EvaluateAnyAsync(existsSpec, ct);
                if (exists)
                    return Result<OrderDto>.Fail("Order number already exists", ErrorType.Conflict);

                order.OrderNumber = command.OrderNumber;
            }

            // Update status if provided
            if (!string.IsNullOrEmpty(command.Status))
            {
                if (Enum.TryParse<OrderStatus>(command.Status, out var newStatus))
                {
                    // Validate state transition
                    var transitionError = ValidateStateTransition(order.Status, newStatus);
                    if (transitionError != null)
                        return Result<OrderDto>.Fail(transitionError, ErrorType.Conflict);

                    order.Status = newStatus;
                }
                else
                {
                    return Result<OrderDto>.Fail($"Invalid status: {command.Status}", ErrorType.Failure);
                }
            }

            order.UpdatedAt = DateTime.UtcNow;
            await _repo.UpdateAsync(order, saveChanges: true, ct);

            return Result<OrderDto>.Ok(order.ToDto());
        }
        catch (Exception ex)
        {
            return Result<OrderDto>.Fail($"Error updating order: {ex.Message}", ErrorType.Failure);
        }
    }

    /// <summary>
    /// Validates order status transitions.
    /// Valid transitions:
    /// - Pending → Processing
    /// - Processing → Shipped
    /// - Shipped → Delivered
    /// - Any state → Cancelled
    /// </summary>
    /// <returns>Error message if transition is invalid, null if valid.</returns>
    private static string? ValidateStateTransition(OrderStatus currentStatus, OrderStatus newStatus)
    {
        // Same status is allowed
        if (currentStatus == newStatus)
            return null;

        // Can always cancel
        if (newStatus == OrderStatus.Cancelled)
            return null;

        // Cannot transition from Cancelled or Delivered
        if (currentStatus == OrderStatus.Cancelled)
            return "Cannot change status of a cancelled order";

        if (currentStatus == OrderStatus.Delivered)
            return "Cannot change status of a delivered order";

        // Valid linear transitions
        if (currentStatus == OrderStatus.Pending && newStatus == OrderStatus.Processing)
            return null;

        if (currentStatus == OrderStatus.Processing && newStatus == OrderStatus.Shipped)
            return null;

        if (currentStatus == OrderStatus.Shipped && newStatus == OrderStatus.Delivered)
            return null;

        // All other transitions are invalid
        return $"Invalid status transition from {currentStatus} to {newStatus}";
    }
}
