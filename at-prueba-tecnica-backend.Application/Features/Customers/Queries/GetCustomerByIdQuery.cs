using at_prueba_tecnica_backend.Application.Features.DTOs;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;

namespace at_prueba_tecnica_backend.Application.Features.Customers.Queries;

/// <summary>
/// Query to retrieve a specific customer by ID.
/// </summary>
public class GetCustomerByIdQuery : IRequest<Result<CustomerDto>>
{
    public Guid Id { get; set; }

    public GetCustomerByIdQuery(Guid id)
    {
        Id = id;
    }
}
