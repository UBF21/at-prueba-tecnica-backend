using at_prueba_tecnica_backend.Application.Features.DTOs;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Products.Commands;

/// <summary>
/// Command to create a new product.
/// </summary>
public record CreateProductCommand(string Name, string? Description, decimal UnitPrice, int Stock)
    : IRequest<Result<ProductDto>>;
