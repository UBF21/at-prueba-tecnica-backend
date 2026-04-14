namespace at_prueba_tecnica_backend.Application.Features.DTOs;

/// <summary>
/// Data Transfer Object for Product.
/// </summary>
public record ProductDto(
    Guid Id,
    int Code,
    string Name,
    string? Description,
    decimal UnitPrice,
    int Stock,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? DeletedAt
);
