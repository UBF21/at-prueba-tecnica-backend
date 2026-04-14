using System.Linq;
using at_prueba_tecnica_backend.Application.Features.Customers.Filters;
using at_prueba_tecnica_backend.Application.Features.Customers.Queries;
using at_prueba_tecnica_backend.Application.Features.DTOs;
using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Flow.Classes.Specification;
using Vali_Flow.Core;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Microsoft.EntityFrameworkCore;

namespace at_prueba_tecnica_backend.Application.Features.Customers.Handlers;

/// <summary>
/// Handler for GetCustomersQuery.
/// Retrieves a paginated list of all active customers.
/// </summary>
public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, Result<(List<CustomerDto>, int)>>
{
    private readonly ICustomerRepository _repository;

    public GetCustomersQueryHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<(List<CustomerDto>, int)>> Handle(GetCustomersQuery query, CancellationToken ct)
    {
        try
        {
            // Get total count without pagination
            var countSpec = new QuerySpecification<Customer>()
                .WithFilter(CustomerFilters.Active());

            var countQueryable = await _repository.EvaluateQueryAsync(countSpec);
            var total = await countQueryable.CountAsync(ct);

            // Get paginated results
            var spec = new QuerySpecification<Customer>()
                .WithFilter(CustomerFilters.Active())
                .WithOrderBy(c => c.Name, ascending: true)
                .WithPagination(query.Page, query.PageSize)
                .WithAsNoTracking(true);

            var queryable = await _repository.EvaluateQueryAsync(spec);
            var customers = await queryable.ToListAsync(ct);

            var dtos = customers.Select(c => new CustomerDto(
                Id: c.Id,
                Code: c.Code,
                Name: c.Name,
                Email: c.Email,
                Phone: c.Phone,
                Address: c.Address,
                CreatedAt: c.CreatedAt,
                UpdatedAt: c.UpdatedAt,
                DeletedAt: c.DeletedAt
            )).ToList();

            return Result<(List<CustomerDto>, int)>.Ok((dtos, total));
        }
        catch (Exception ex)
        {
            return Result<(List<CustomerDto>, int)>.Fail($"Error retrieving customers: {ex.Message}", ErrorType.Failure);
        }
    }
}
