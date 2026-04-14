using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Orders.Commands;

/// <summary>
/// Command to delete an order (soft delete via DeletedAt timestamp).
/// </summary>
public record DeleteOrderCommand(int Id) : IRequest<Result<bool>>;
