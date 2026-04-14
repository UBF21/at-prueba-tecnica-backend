using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Products.Commands;

/// <summary>
/// Command to delete a product (soft delete via DeletedAt timestamp).
/// </summary>
public record DeleteProductCommand(int Id) : IRequest<Result<bool>>;
