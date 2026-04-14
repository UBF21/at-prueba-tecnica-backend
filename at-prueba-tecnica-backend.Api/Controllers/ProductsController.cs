using at_prueba_tecnica_backend.Api.Extensions;
using at_prueba_tecnica_backend.Api.Responses;
using at_prueba_tecnica_backend.Application.Features.DTOs;
using at_prueba_tecnica_backend.Application.Features.Products.Commands;
using at_prueba_tecnica_backend.Application.Features.Products.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vali_Mediator.AspNetCore;
using Vali_Mediator.Core.General;
using Vali_Mediator.Core.General.Mediator;

namespace at_prueba_tecnica_backend.Api.Controllers;

/// <summary>
/// Endpoints for product inventory management.
/// Requires JWT Bearer token authentication.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IValiMediator _mediator;

    public ProductsController(IValiMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves all active products with pagination.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<ProductDto>>> GetProducts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var query = new GetProductsQuery(page, pageSize);
        var result = await _mediator.Send(query, ct);
        var response = result.ToPaginatedResponse(page, pageSize);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    /// <summary>
    /// Retrieves a specific product by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> GetProductById(Guid id, CancellationToken ct)
    {
        var query = new GetProductByIdQuery(id);
        var result = await _mediator.Send(query, ct);
        var response = result.ToApiResponse();
        return response.Success ? Ok(response) : NotFound(response);
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProductDto>>> CreateProduct(
        [FromBody] CreateProductCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        var response = result.ToApiResponse();
        return response.Success ? Ok(response) : BadRequest(response);
    }

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> UpdateProduct(
        Guid id,
        [FromBody] UpdateProductCommand command,
        CancellationToken ct)
    {
        var cmd = command with { Id = id };
        var result = await _mediator.Send(cmd, ct);
        var response = result.ToApiResponse();
        return response.Success ? Ok(response) : BadRequest(response);
    }

    /// <summary>
    /// Deletes a product (soft delete).
    /// The product is marked as deleted but not removed from the database.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteProduct(Guid id, CancellationToken ct)
    {
        var command = new DeleteProductCommand(id);
        var result = await _mediator.Send(command, ct);
        var response = result.ToApiResponse();
        return response.Success ? Ok(response) : NotFound(response);
    }
}
