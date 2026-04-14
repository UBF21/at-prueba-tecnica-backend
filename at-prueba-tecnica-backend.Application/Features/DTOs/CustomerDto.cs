namespace at_prueba_tecnica_backend.Application.Features.DTOs;

/// <summary>
/// Data Transfer Object for Customer.
/// Exposes Code (int) as the public identifier.
/// </summary>
public record CustomerDto(
    int Code,
    string Name,
    string Email,
    string? Phone,
    string? Address,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? DeletedAt
);
