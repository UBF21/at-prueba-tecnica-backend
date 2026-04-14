using at_prueba_tecnica_backend.Application.Features.Customers.Filters;
using at_prueba_tecnica_backend.Application.Features.Customers.Queries;
using at_prueba_tecnica_backend.Application.Features.DTOs;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Flow.Classes.Specification;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Customers.Handlers;

/// <summary>
/// Handler for GetCustomerByIdQuery.
/// Retrieves a specific customer by ID.
/// </summary>
public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Result<CustomerDto>>
{
    private readonly ICustomerRepository _repository;

    public GetCustomerByIdQueryHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<CustomerDto>> Handle(GetCustomerByIdQuery query, CancellationToken ct)
    {
        try
        {
            var spec = new BasicSpecification<Domain.Entities.Customer>()
                .WithFilter(CustomerFilters.ById(query.Id))
                .WithAsNoTracking(true);

            var customer = await _repository.EvaluateGetFirstAsync(spec, ct);

            if (customer is null)
                return Result<CustomerDto>.Fail("Customer not found", ErrorType.NotFound);

            var dto = new CustomerDto(
                Id: customer.Id,
                Code: customer.Code,
                Name: customer.Name,
                Email: customer.Email,
                Phone: customer.Phone,
                Address: customer.Address,
                CreatedAt: customer.CreatedAt,
                UpdatedAt: customer.UpdatedAt,
                DeletedAt: customer.DeletedAt
            );

            return Result<CustomerDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return Result<CustomerDto>.Fail($"Error retrieving customer: {ex.Message}", ErrorType.Failure);
        }
    }
}
