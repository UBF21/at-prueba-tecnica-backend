using System.Linq;
using Vali_Flow.Core;
using Vali_Flow.Classes.Specification;
using at_prueba_tecnica_backend.Application.Features.DTOs;
using at_prueba_tecnica_backend.Application.Features.Mappings;
using at_prueba_tecnica_backend.Application.Features.Products.Filters;
using at_prueba_tecnica_backend.Application.Features.Products.Queries;
using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Vali_Flow.Abstractions;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Products.Handlers;

/// <summary>
/// Handler for GetProductsQuery.
/// Retrieves products with pagination using Vali-Flow specifications.
/// Returns tuple with (products, totalCount) for pagination metadata.
/// </summary>
public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<(List<ProductDto> Data, int Total)>>
{
    private readonly IProductRepository _repo;

    public GetProductsQueryHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<(List<ProductDto> Data, int Total)>> Handle(GetProductsQuery query, CancellationToken ct)
    {
        try
        {
            // Get total count without pagination
            var countSpec = new BasicSpecification<Product>()
                .WithFilter(ProductFilters.Active());

            var countQueryable = await _repo.EvaluateQueryAsync(countSpec);
            var total = await countQueryable.CountAsync(ct);

            // Get paginated results
            var spec = new QuerySpecification<Product>()
                .WithFilter(ProductFilters.Active())
                .WithOrderBy(p => p.CreatedAt, ascending: false)
                .WithPagination(query.Page, query.PageSize)
                .WithAsNoTracking(true);

            var queryable = await _repo.EvaluateQueryAsync(spec);
            var products = await queryable.ToListAsync(ct);

            var dtos = products.Select(p => p.ToDto()).ToList();
            return Result<(List<ProductDto>, int)>.Ok((dtos, total));
        }
        catch (Exception ex)
        {
            return Result<(List<ProductDto>, int)>.Fail($"Error retrieving products: {ex.Message}", ErrorType.Failure);
        }
    }
}
