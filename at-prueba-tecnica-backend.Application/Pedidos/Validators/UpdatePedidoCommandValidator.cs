using at_prueba_tecnica_backend.Application.Pedidos.Commands;
using at_prueba_tecnica_backend.Domain.Enums;
using Vali_Validation.Core.Validators;

namespace at_prueba_tecnica_backend.Application.Pedidos.Validators;

/// <summary>
/// Validador para UpdatePedidoCommand.
/// </summary>
public class UpdatePedidoCommandValidator : AbstractValidator<UpdatePedidoCommand>
{
    protected override CascadeMode GlobalCascadeMode => CascadeMode.StopOnFirstFailure;

    public UpdatePedidoCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El ID del pedido debe ser mayor a 0");

        RuleFor(x => x.Total)
            .GreaterThan(0m).WithMessage("El total debe ser mayor a 0");

        RuleFor(x => x.Estado)
            .IsEnum<EstadoPedido>().WithMessage("El estado debe ser un valor válido");
    }
}
