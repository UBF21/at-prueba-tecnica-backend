namespace at_prueba_tecnica_backend.Application.Features.DTOs;

/// <summary>
/// Data Transfer Object for Product.
/// Exposes Code (int) as the public identifier.
/// </summary>
public record ProductDto(
    int Code,
    string Name,
    string? Description,
    decimal UnitPrice,
    int Stock,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? DeletedAt
);
