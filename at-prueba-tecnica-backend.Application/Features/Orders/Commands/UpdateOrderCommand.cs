using at_prueba_tecnica_backend.Application.Features.DTOs;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Orders.Commands;

/// <summary>
/// Command to update an existing order.
/// </summary>
public record UpdateOrderCommand(Guid Id, string? OrderNumber = null, string? Status = null)
    : IRequest<Result<OrderDto>>;
