using at_prueba_tecnica_backend.Api.Extensions;
using at_prueba_tecnica_backend.Api.Requests;
using at_prueba_tecnica_backend.Api.Responses;
using at_prueba_tecnica_backend.Application.Features.DTOs;
using at_prueba_tecnica_backend.Application.Features.Orders.Commands;
using at_prueba_tecnica_backend.Application.Features.Orders.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vali_Mediator.AspNetCore;
using Vali_Mediator.Core.General;
using Vali_Mediator.Core.General.Mediator;

namespace at_prueba_tecnica_backend.Api.Controllers;

/// <summary>
/// Endpoints for order management.
/// Requires JWT Bearer token authentication.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IValiMediator _mediator;

    public OrdersController(IValiMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves all orders with pagination and optional status filtering.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<OrderDto>>> GetOrders(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null,
        CancellationToken ct = default)
    {
        var query = new GetOrdersQuery(page, pageSize, status);
        var result = await _mediator.Send(query, ct);
        var response = result.ToPaginatedResponse(page, pageSize);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    /// <summary>
    /// Retrieves a specific order by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> GetOrderById(Guid id, CancellationToken ct)
    {
        var query = new GetOrderByIdQuery(id);
        var result = await _mediator.Send(query, ct);
        var response = result.ToApiResponse();
        return response.Success ? Ok(response) : NotFound(response);
    }

    /// <summary>
    /// Creates a new order.
    /// OrderNumber must be unique.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<OrderDto>>> CreateOrder(
        [FromBody] CreateOrderCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        var response = result.ToApiResponse();
        return response.Success ? Ok(response) : BadRequest(response);
    }

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> UpdateOrder(
        Guid id,
        [FromBody] UpdateOrderCommand command,
        CancellationToken ct)
    {
        var cmd = command with { Id = id };
        var result = await _mediator.Send(cmd, ct);
        var response = result.ToApiResponse();
        return response.Success ? Ok(response) : BadRequest(response);
    }

    /// <summary>
    /// Deletes an order (soft delete).
    /// The order is marked as deleted but not removed from the database.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteOrder(Guid id, CancellationToken ct)
    {
        var command = new DeleteOrderCommand(id);
        var result = await _mediator.Send(command, ct);
        var response = result.ToApiResponse();
        return response.Success ? Ok(response) : NotFound(response);
    }

    /// <summary>
    /// Adds an item to an existing order.
    /// The order must not be in Cancelled or Delivered status.
    /// The product must have sufficient stock.
    /// </summary>
    [HttpPost("{orderId}/items")]
    public async Task<IActionResult> AddOrderItem(
        Guid orderId,
        [FromBody] AddOrderItemRequest request,
        CancellationToken ct)
    {
        var command = new CreateOrderItemCommand(orderId, request.ProductId, request.Quantity);
        var result = await _mediator.Send(command, ct);
        return result.ToActionResult();
    }
}
