using at_prueba_tecnica_backend.Api.Extensions;
using at_prueba_tecnica_backend.Api.Responses;
using at_prueba_tecnica_backend.Application.Features.Customers.Commands;
using at_prueba_tecnica_backend.Application.Features.Customers.Queries;
using at_prueba_tecnica_backend.Application.Features.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vali_Mediator.Core.General.Mediator;
using ComboBoxDto = at_prueba_tecnica_backend.Api.Responses.ComboBoxDto;

namespace at_prueba_tecnica_backend.Api.Controllers;

/// <summary>
/// Endpoints for customer management.
/// Requires JWT Bearer token authentication.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IValiMediator _mediator;

    public CustomersController(IValiMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves all customers with pagination.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<CustomerDto>>> GetCustomers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var query = new GetCustomersQuery(page, pageSize);
        var result = await _mediator.Send(query, ct);
        var response = result.ToPaginatedResponse(page, pageSize);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    /// <summary>
    /// Retrieves a specific customer by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> GetCustomer(Guid id, CancellationToken ct = default)
    {
        var query = new GetCustomerByIdQuery(id);
        var result = await _mediator.Send(query, ct);
        var response = result.ToApiResponse();
        return response.Success ? Ok(response) : NotFound(response);
    }

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CreateCustomerCommand.Response>>> CreateCustomer(
        [FromBody] CreateCustomerCommand command,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        var response = result.ToApiResponse();
        return response.Success
            ? CreatedAtAction(nameof(GetCustomer), new { id = response.Data?.Id }, response)
            : BadRequest(response);
    }

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<UpdateCustomerCommand.Response>>> UpdateCustomer(
        Guid id,
        [FromBody] UpdateCustomerCommand command,
        CancellationToken ct = default)
    {
        command.Id = id;
        var result = await _mediator.Send(command, ct);
        var response = result.ToApiResponse();
        return response.Success ? Ok(response) : BadRequest(response);
    }

    /// <summary>
    /// Deletes (soft delete) a customer.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteCustomer(Guid id, CancellationToken ct = default)
    {
        var command = new DeleteCustomerCommand(id);
        var result = await _mediator.Send(command, ct);
        var response = result.ToApiResponse();
        return response.Success ? Ok(response) : BadRequest(response);
    }

    /// <summary>
    /// Retrieves all customers as a combobox list (for dropdowns).
    /// </summary>
    [HttpGet("combobox/list")]
    public async Task<ActionResult<ListResponse<ComboBoxDto>>> GetCustomersComboBox(CancellationToken ct = default)
    {
        var query = new GetCustomersQuery(1, 1000);
        var result = await _mediator.Send(query, ct);

        if (!result.IsSuccess)
            return BadRequest(ListResponse<ComboBoxDto>.Fail(result.Error ?? ""));

        var (customers, _) = result.Value;
        var comboBoxItems = customers
            .Select(c => new ComboBoxDto(c.Id.ToString(), c.Name))
            .ToList();

        return Ok(ListResponse<ComboBoxDto>.Ok(comboBoxItems));
    }
}
