using at_prueba_tecnica_backend.Application.Features.DTOs;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Orders.Commands;

/// <summary>
/// Command to create a new order.
/// </summary>
public record CreateOrderCommand(string OrderNumber, int CustomerId)
    : IRequest<Result<OrderDto>>;
