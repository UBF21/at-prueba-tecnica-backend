using at_prueba_tecnica_backend.Application.Features.Customers.Commands;
using at_prueba_tecnica_backend.Application.Features.Customers.Filters;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Flow.Classes.Specification;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Customers.Handlers;

/// <summary>
/// Handler for UpdateCustomerCommand.
/// Updates an existing customer in the database.
/// </summary>
public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result<UpdateCustomerCommand.Response>>
{
    private readonly ICustomerRepository _repository;

    public UpdateCustomerCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<UpdateCustomerCommand.Response>> Handle(UpdateCustomerCommand command, CancellationToken ct)
    {
        try
        {
            var spec = new BasicSpecification<Domain.Entities.Customer>()
                .WithFilter(CustomerFilters.ById(command.Id))
                .WithAsNoTracking(false);

            var customer = await _repository.EvaluateGetFirstAsync(spec, ct);

            if (customer is null)
                return Result<UpdateCustomerCommand.Response>.Fail("Customer not found", ErrorType.NotFound);

            customer.Name = command.Name;
            customer.Email = command.Email;
            customer.Phone = command.Phone;
            customer.Address = command.Address;
            customer.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(customer, saveChanges: true, ct);

            var response = new UpdateCustomerCommand.Response
            {
                Id = customer.Id,
                Code = customer.Code,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
            };

            return Result<UpdateCustomerCommand.Response>.Ok(response);
        }
        catch (Exception ex)
        {
            return Result<UpdateCustomerCommand.Response>.Fail($"Error updating customer: {ex.Message}", ErrorType.Failure);
        }
    }
}
