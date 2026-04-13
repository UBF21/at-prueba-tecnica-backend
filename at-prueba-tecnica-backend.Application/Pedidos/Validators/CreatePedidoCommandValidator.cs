using at_prueba_tecnica_backend.Application.Pedidos.Commands;
using Vali_Validation.Core.Validators;

namespace at_prueba_tecnica_backend.Application.Pedidos.Validators;

/// <summary>
/// Validador para CreatePedidoCommand usando Vali-Validation.
/// Valida el comando antes de ser procesado por el handler.
/// </summary>
public class CreatePedidoCommandValidator : AbstractValidator<CreatePedidoCommand>
{
    protected override CascadeMode GlobalCascadeMode => CascadeMode.StopOnFirstFailure;

    public CreatePedidoCommandValidator()
    {
        RuleFor(x => x.NumeroPedido)
            .NotEmpty().WithMessage("El número de pedido es requerido")
            .MaximumLength(50).WithMessage("El número de pedido no puede exceder 50 caracteres");

        RuleFor(x => x.Total)
            .GreaterThan(0m).WithMessage("El total debe ser mayor a 0");

        RuleFor(x => x.ClienteId)
            .GreaterThan(0).WithMessage("El ID del cliente debe ser mayor a 0");
    }
}
