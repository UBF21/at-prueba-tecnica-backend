using at_prueba_tecnica_backend.Application.Features.Products.Commands;
using FluentValidation;

namespace at_prueba_tecnica_backend.Application.Features.Products.Validators;

/// <summary>
/// Validator for DeleteProductCommand.
/// </summary>
public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Product ID must be greater than 0");
    }
}
