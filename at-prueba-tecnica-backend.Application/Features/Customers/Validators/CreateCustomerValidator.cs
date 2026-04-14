using at_prueba_tecnica_backend.Application.Features.Customers.Commands;
using at_prueba_tecnica_backend.Application.Features.Customers.Filters;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Flow.Classes.Specification;
using Vali_Validation.Core.Validators;

namespace at_prueba_tecnica_backend.Application.Features.Customers.Validators;

/// <summary>
/// Validator for CreateCustomerCommand using fluent validation.
/// </summary>
public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    private readonly ICustomerRepository _repository;

    public CreateCustomerValidator(ICustomerRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido")
            .Email().WithMessage("El email debe ser válido")
            .MaximumLength(100).WithMessage("El email no puede exceder 100 caracteres")
            .MustAsync(BeUniqueEmail).WithMessage("El email ya está registrado");

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("El teléfono no puede exceder 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("La dirección no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Address));
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken ct)
    {
        var spec = new BasicSpecification<Domain.Entities.Customer>()
            .WithFilter(CustomerFilters.ByEmail(email));

        var exists = await _repository.EvaluateAnyAsync(spec, ct);
        return !exists;
    }
}
