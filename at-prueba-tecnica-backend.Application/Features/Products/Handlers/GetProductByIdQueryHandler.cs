using Vali_Flow.Core;
using at_prueba_tecnica_backend.Application.Features.DTOs;
using at_prueba_tecnica_backend.Application.Features.Mappings;
using at_prueba_tecnica_backend.Application.Features.Products.Filters;
using at_prueba_tecnica_backend.Application.Features.Products.Queries;
using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Flow.Abstractions;
using Vali_Flow.Classes.Specification;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Products.Handlers;

/// <summary>
/// Handler for GetProductByIdQuery.
/// Retrieves a specific product by ID using Vali-Flow BasicSpecification.
/// </summary>
public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    private readonly IProductRepository _repo;

    public GetProductByIdQueryHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery query, CancellationToken ct)
    {
        try
        {
            var spec = new BasicSpecification<Product>()
                .WithFilter(ProductFilters.ById(query.Id))
                .WithAsNoTracking(true);

            var product = await _repo.EvaluateGetFirstAsync(spec, ct);

            if (product is null)
                return Result<ProductDto>.Fail("Product not found", ErrorType.NotFound);

            return Result<ProductDto>.Ok(product.ToDto());
        }
        catch (Exception ex)
        {
            return Result<ProductDto>.Fail($"Error retrieving product: {ex.Message}", ErrorType.Failure);
        }
    }
}
