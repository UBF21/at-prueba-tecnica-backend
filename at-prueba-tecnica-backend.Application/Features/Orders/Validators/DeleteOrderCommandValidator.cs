using at_prueba_tecnica_backend.Application.Features.Orders.Commands;
using Vali_Validation.Core.Validators;

namespace at_prueba_tecnica_backend.Application.Features.Orders.Validators;

/// <summary>
/// Validator for DeleteOrderCommand.
/// </summary>
public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Order ID must be valid");
    }
}
