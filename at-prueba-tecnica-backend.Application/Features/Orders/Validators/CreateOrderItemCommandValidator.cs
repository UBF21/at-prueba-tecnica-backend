using at_prueba_tecnica_backend.Application.Features.Orders.Commands;
using FluentValidation;

namespace at_prueba_tecnica_backend.Application.Features.Orders.Validators;

/// <summary>
/// Validator for CreateOrderItemCommand.
/// </summary>
public class CreateOrderItemCommandValidator : AbstractValidator<CreateOrderItemCommand>
{
    public CreateOrderItemCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .GreaterThan(0).WithMessage("Order ID must be greater than 0");

        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Product ID must be greater than 0");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}
