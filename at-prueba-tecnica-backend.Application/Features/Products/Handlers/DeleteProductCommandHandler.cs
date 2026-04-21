using Vali_Flow.Core;
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
/// Handler for DeleteProductCommand.
/// Performs soft delete by setting DeletedAt timestamp.
/// </summary>
public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<bool>>
{
    private readonly IProductRepository _repo;

    public DeleteProductCommandHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<bool>> Handle(DeleteProductCommand command, CancellationToken ct)
    {
        try
        {
            var spec = new BasicSpecification<Product>()
                .WithFilter(ProductFilters.ById(command.Id));

            var product = await _repo.EvaluateGetFirstAsync(spec, ct);

            if (product is null)
                return Result<bool>.Fail("Product not found", ErrorType.NotFound);

            product.Delete();
            await _repo.UpdateAsync(product, saveChanges: true, ct);

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Error deleting product: {ex.Message}", ErrorType.Failure);
        }
    }
}
