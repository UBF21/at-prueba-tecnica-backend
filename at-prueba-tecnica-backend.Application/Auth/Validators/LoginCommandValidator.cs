using at_prueba_tecnica_backend.Application.Auth.Commands;
using Vali_Validation.Core.Validators;

namespace at_prueba_tecnica_backend.Application.Auth.Validators;

/// <summary>
/// Validador para LoginCommand.
/// Valida que email y contraseña cumplan con requisitos básicos.
/// </summary>
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    protected override CascadeMode GlobalCascadeMode => CascadeMode.StopOnFirstFailure;

    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido")
            .Email().WithMessage("El email debe ser válido");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres");
    }
}
