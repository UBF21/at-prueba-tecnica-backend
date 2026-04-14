using Vali_Flow.Core;
using at_prueba_tecnica_backend.Application.Features.DTOs;
using at_prueba_tecnica_backend.Application.Features.Mappings;
using at_prueba_tecnica_backend.Application.Features.Products.Commands;
using at_prueba_tecnica_backend.Application.Features.Products.Filters;
using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Flow.Abstractions;
using Vali_Flow.Classes.Specification;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Products.Handlers;

/// <summary>
/// Handler for UpdateProductCommand.
/// Updates product details: name, description, price, and stock.
/// </summary>
public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<ProductDto>>
{
    private readonly IProductRepository _repo;

    public UpdateProductCommandHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<ProductDto>> Handle(UpdateProductCommand command, CancellationToken ct)
    {
        try
        {
            var spec = new BasicSpecification<Product>()
                .WithFilter(ProductFilters.ById(command.Id));

            var product = await _repo.EvaluateGetFirstAsync(spec, ct);

            if (product is null)
                return Result<ProductDto>.Fail("Product not found", ErrorType.NotFound);

            if (!string.IsNullOrEmpty(command.Name))
                product.Name = command.Name;

            if (command.Description != null)
                product.Description = command.Description;

            if (command.UnitPrice.HasValue)
            {
                if (command.UnitPrice <= 0)
                    return Result<ProductDto>.Fail("Unit price must be greater than 0", ErrorType.Failure);
                product.UnitPrice = command.UnitPrice.Value;
            }

            if (command.Stock.HasValue)
            {
                if (command.Stock < 0)
                    return Result<ProductDto>.Fail("Stock cannot be negative", ErrorType.Failure);
                product.Stock = command.Stock.Value;
            }

            product.UpdatedAt = DateTime.UtcNow;
            await _repo.UpdateAsync(product, saveChanges: true, ct);

            return Result<ProductDto>.Ok(product.ToDto());
        }
        catch (Exception ex)
        {
            return Result<ProductDto>.Fail($"Error updating product: {ex.Message}", ErrorType.Failure);
        }
    }
}
