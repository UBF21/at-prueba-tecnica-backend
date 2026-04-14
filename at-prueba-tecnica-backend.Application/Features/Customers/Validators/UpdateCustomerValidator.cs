using at_prueba_tecnica_backend.Application.Features.Customers.Commands;
using at_prueba_tecnica_backend.Application.Features.Customers.Filters;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Validation.Core.Validators;
using Vali_Flow.Classes.Specification;

namespace at_prueba_tecnica_backend.Application.Features.Customers.Validators;

/// <summary>
/// Validator for UpdateCustomerCommand using fluent validation.
/// </summary>
public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
{
    private readonly ICustomerRepository _repository;

    public UpdateCustomerValidator(ICustomerRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El ID del cliente debe ser válido");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido")
            .Email().WithMessage("El email debe ser válido")
            .MaximumLength(100).WithMessage("El email no puede exceder 100 caracteres")
            .MustAsync(BeUniqueEmailForUpdate);

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("El teléfono no puede exceder 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("La dirección no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Address));
    }

    private async Task<bool> BeUniqueEmailForUpdate(string email, CancellationToken ct)
    {
        var spec = new BasicSpecification<Domain.Entities.Customer>()
            .WithFilter(CustomerFilters.ByEmail(email));

        var exists = await _repository.EvaluateAnyAsync(spec, ct);

        // Allow if no customer with this email exists, or if it's the same customer
        if (!exists) return true;

        var customer = await _repository.EvaluateGetFirstAsync(
            new BasicSpecification<Domain.Entities.Customer>()
                .WithFilter(CustomerFilters.ByEmail(email)), ct);

        return customer?.Email == email;
    }
}
