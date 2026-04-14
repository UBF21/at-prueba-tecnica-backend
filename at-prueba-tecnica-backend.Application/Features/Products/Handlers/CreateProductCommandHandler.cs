using at_prueba_tecnica_backend.Application.Features.DTOs;
using at_prueba_tecnica_backend.Application.Features.Mappings;
using at_prueba_tecnica_backend.Application.Features.Products.Commands;
using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Products.Handlers;

/// <summary>
/// Handler for CreateProductCommand.
/// Creates a new product with inventory tracking.
/// </summary>
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductDto>>
{
    private readonly IProductRepository _repo;

    public CreateProductCommandHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<ProductDto>> Handle(CreateProductCommand command, CancellationToken ct)
    {
        try
        {
            if (command.UnitPrice <= 0)
                return Result<ProductDto>.Fail("Unit price must be greater than 0", ErrorType.Failure);

            if (command.Stock < 0)
                return Result<ProductDto>.Fail("Stock cannot be negative", ErrorType.Failure);

            var newProduct = new Product
            {
                Code = Guid.NewGuid().ToString("N").Substring(0, 36),
                Name = command.Name,
                Description = command.Description,
                UnitPrice = command.UnitPrice,
                Stock = command.Stock,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(newProduct, saveChanges: true, ct);

            return Result<ProductDto>.Ok(newProduct.ToDto());
        }
        catch (Exception ex)
        {
            return Result<ProductDto>.Fail($"Error creating product: {ex.Message}", ErrorType.Failure);
        }
    }
}
