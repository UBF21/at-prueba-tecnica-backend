using at_prueba_tecnica_backend.Application.Features.DTOs;
using at_prueba_tecnica_backend.Domain.Entities;

namespace at_prueba_tecnica_backend.Application.Features.Mappings;

/// <summary>
/// Mapping extensions for Product entity.
/// </summary>
public static class ProductMappingExtensions
{
    /// <summary>
    /// Maps a Product entity to its DTO.
    /// </summary>
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(
            Id: product.Id,
            Code: product.Code,
            Name: product.Name,
            Description: product.Description,
            UnitPrice: product.UnitPrice,
            Stock: product.Stock,
            CreatedAt: product.CreatedAt,
            UpdatedAt: product.UpdatedAt,
            DeletedAt: product.DeletedAt
        );
    }
}
