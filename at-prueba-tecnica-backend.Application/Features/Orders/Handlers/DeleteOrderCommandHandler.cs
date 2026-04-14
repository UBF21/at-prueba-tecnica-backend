using Vali_Flow.Core;
using at_prueba_tecnica_backend.Application.Features.Orders.Commands;
using at_prueba_tecnica_backend.Application.Features.Orders.Filters;
using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Flow.Abstractions;
using Vali_Flow.Classes.Specification;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Orders.Handlers;

/// <summary>
/// Handler for DeleteOrderCommand.
/// Performs soft delete by setting DeletedAt timestamp.
/// </summary>
public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Result<bool>>
{
    private readonly IOrderRepository _repo;

    public DeleteOrderCommandHandler(IOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<bool>> Handle(DeleteOrderCommand command, CancellationToken ct)
    {
        try
        {
            var spec = new BasicSpecification<Order>()
                .WithFilter(OrderFilters.ById(command.Id));

            var order = await _repo.EvaluateGetFirstAsync(spec, ct);

            if (order is null)
                return Result<bool>.Fail("Order not found", ErrorType.NotFound);

            order.DeletedAt = DateTime.UtcNow;
            await _repo.UpdateAsync(order, saveChanges: true, ct);

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Error deleting order: {ex.Message}", ErrorType.Failure);
        }
    }
}
