using at_prueba_tecnica_backend.Domain.Entities;
using Vali_Flow.Core.Builder;

namespace at_prueba_tecnica_backend.Application.Features.Products.Filters;

/// <summary>
/// Reusable filters for Product queries using Vali-Flow.
/// Builds dynamic LINQ expressions without external dependencies.
/// </summary>
public static class ProductFilters
{
    /// <summary>
    /// Base filter: excludes deleted products (soft delete).
    /// </summary>
    public static ValiFlowQuery<Product> Active() =>
        new ValiFlowQuery<Product>().IsNull(p => p.DeletedAt);

    /// <summary>
    /// Filter by product ID.
    /// </summary>
    public static ValiFlowQuery<Product> ById(Guid id) =>
        Active().And().EqualTo(p => p.Id, id);

    /// <summary>
    /// Filter by product code.
    /// </summary>
    public static ValiFlowQuery<Product> ByCode(int code) =>
        Active().And().EqualTo(p => p.Code, code);
}
