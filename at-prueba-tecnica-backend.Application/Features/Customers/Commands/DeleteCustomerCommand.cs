using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;

namespace at_prueba_tecnica_backend.Application.Features.Customers.Commands;

/// <summary>
/// Command to delete (soft delete) a customer.
/// </summary>
public class DeleteCustomerCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }

    public DeleteCustomerCommand(Guid id)
    {
        Id = id;
    }
}
