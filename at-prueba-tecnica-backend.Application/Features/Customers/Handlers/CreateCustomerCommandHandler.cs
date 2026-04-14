using at_prueba_tecnica_backend.Application.Features.Customers.Commands;
using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Customers.Handlers;

/// <summary>
/// Handler for CreateCustomerCommand.
/// Creates a new customer in the database.
/// </summary>
public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<CreateCustomerCommand.Response>>
{
    private readonly ICustomerRepository _repository;

    public CreateCustomerCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<CreateCustomerCommand.Response>> Handle(CreateCustomerCommand command, CancellationToken ct)
    {
        var customer = new Customer
        {
            Name = command.Name,
            Email = command.Email,
            Phone = command.Phone,
            Address = command.Address,
        };

        await _repository.AddAsync(customer, saveChanges: true, ct);

        var response = new CreateCustomerCommand.Response
        {
            Id = customer.Id,
            Code = customer.Code,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address,
        };

        return Result<CreateCustomerCommand.Response>.Ok(response);
    }
}
