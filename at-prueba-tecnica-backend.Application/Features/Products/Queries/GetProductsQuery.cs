using at_prueba_tecnica_backend.Application.Features.DTOs;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Products.Queries;

/// <summary>
/// Query to retrieve a list of products with pagination.
/// Returns tuple with (products, totalCount) to support pagination metadata.
/// </summary>
public record GetProductsQuery(int Page = 1, int PageSize = 10)
    : IRequest<Result<(List<ProductDto> Data, int Total)>>;
