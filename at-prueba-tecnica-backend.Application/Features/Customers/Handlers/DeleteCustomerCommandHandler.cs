using at_prueba_tecnica_backend.Application.Features.Customers.Commands;
using at_prueba_tecnica_backend.Application.Features.Customers.Filters;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Flow.Classes.Specification;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Customers.Handlers;

/// <summary>
/// Handler for DeleteCustomerCommand.
/// Performs soft delete on a customer (marks DeletedAt timestamp).
/// </summary>
public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result<bool>>
{
    private readonly ICustomerRepository _repository;

    public DeleteCustomerCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(DeleteCustomerCommand command, CancellationToken ct)
    {
        try
        {
            var spec = new BasicSpecification<Domain.Entities.Customer>()
                .WithFilter(CustomerFilters.ById(command.Id))
                .WithAsNoTracking(false);

            var customer = await _repository.EvaluateGetFirstAsync(spec, ct);

            if (customer is null)
                return Result<bool>.Fail("Customer not found", ErrorType.NotFound);

            customer.Delete();
            await _repository.UpdateAsync(customer, saveChanges: true, ct);

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Error deleting customer: {ex.Message}", ErrorType.Failure);
        }
    }
}
