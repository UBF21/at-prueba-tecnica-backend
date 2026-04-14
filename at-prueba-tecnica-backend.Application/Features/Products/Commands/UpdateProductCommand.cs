using at_prueba_tecnica_backend.Application.Features.DTOs;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Products.Commands;

/// <summary>
/// Command to update an existing product.
/// </summary>
public record UpdateProductCommand(Guid Id, string? Name = null, string? Description = null, decimal? UnitPrice = null, int? Stock = null)
    : IRequest<Result<ProductDto>>;
