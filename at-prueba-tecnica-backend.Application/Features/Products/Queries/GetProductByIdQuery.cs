using at_prueba_tecnica_backend.Application.Features.DTOs;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Products.Queries;

/// <summary>
/// Query to retrieve a specific product by ID.
/// </summary>
public record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductDto>>;
