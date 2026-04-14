using at_prueba_tecnica_backend.Application.Features.Orders.Commands;
using Vali_Validation.Core.Validators;

namespace at_prueba_tecnica_backend.Application.Features.Orders.Validators;

/// <summary>
/// Validator for CreateOrderItemCommand.
/// </summary>
public class CreateOrderItemCommandValidator : AbstractValidator<CreateOrderItemCommand>
{
    public CreateOrderItemCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order ID must be valid");

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID must be valid");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}
