namespace at_prueba_tecnica_backend.Application.Features.DTOs;

/// <summary>
/// Data Transfer Object for Product.
/// Exposes Code as the shareable identifier across environments.
/// </summary>
public record ProductDto(
    string Code,
    string Name,
    string? Description,
    decimal UnitPrice,
    int Stock,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? DeletedAt
);
