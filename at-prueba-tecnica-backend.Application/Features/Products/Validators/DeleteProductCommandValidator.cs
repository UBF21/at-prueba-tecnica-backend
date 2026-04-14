using at_prueba_tecnica_backend.Application.Features.Products.Commands;
using Vali_Validation.Core.Validators;

namespace at_prueba_tecnica_backend.Application.Features.Products.Validators;

/// <summary>
/// Validator for DeleteProductCommand.
/// </summary>
public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product ID must be valid");
    }
}
