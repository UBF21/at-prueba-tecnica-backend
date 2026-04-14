using at_prueba_tecnica_backend.Application.Features.Orders.Commands;
using Vali_Validation.Core.Validators;

namespace at_prueba_tecnica_backend.Application.Features.Orders.Validators;

/// <summary>
/// Validator for CreateOrderCommand.
/// </summary>
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.OrderNumber)
            .NotEmpty().WithMessage("Order number is required")
            .MaximumLength(100).WithMessage("Order number must not exceed 100 characters");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID must be valid");
    }
}
