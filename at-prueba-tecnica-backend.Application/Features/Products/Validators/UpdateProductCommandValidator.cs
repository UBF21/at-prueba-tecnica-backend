using at_prueba_tecnica_backend.Application.Features.Products.Commands;
using Vali_Validation.Core.Validators;

namespace at_prueba_tecnica_backend.Application.Features.Products.Validators;

/// <summary>
/// Validator for UpdateProductCommand.
/// </summary>
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product ID must be valid");

        RuleFor(x => x.Name)
            .MaximumLength(255).WithMessage("Product name must not exceed 255 characters")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than 0")
            .When(x => x.UnitPrice.HasValue);

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative")
            .When(x => x.Stock.HasValue);
    }
}
