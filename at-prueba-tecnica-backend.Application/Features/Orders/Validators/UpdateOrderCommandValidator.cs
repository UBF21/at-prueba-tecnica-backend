using at_prueba_tecnica_backend.Application.Features.Orders.Commands;
using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Enums;
using Vali_Validation.Core.Validators;

namespace at_prueba_tecnica_backend.Application.Features.Orders.Validators;

/// <summary>
/// Validator for UpdateOrderCommand.
/// </summary>
public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Order ID must be valid");

        RuleFor(x => x.OrderNumber)
            .MaximumLength(100).WithMessage("Order number must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.OrderNumber));

        RuleFor(x => x.Status)
            .Must(status => string.IsNullOrEmpty(status) || IsValidOrderStatus(status))
            .WithMessage("Invalid order status")
            .When(x => !string.IsNullOrEmpty(x.Status));
    }

    private static bool IsValidOrderStatus(string status)
    {
        return Enum.TryParse<OrderStatus>(status, ignoreCase: true, out _);
    }
}
