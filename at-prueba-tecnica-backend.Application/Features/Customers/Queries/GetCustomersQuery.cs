using at_prueba_tecnica_backend.Application.Features.DTOs;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;

namespace at_prueba_tecnica_backend.Application.Features.Customers.Queries;

/// <summary>
/// Query to retrieve paginated list of customers.
/// </summary>
public class GetCustomersQuery : IRequest<Result<(List<CustomerDto> Data, int Total)>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public GetCustomersQuery()
    {
    }

    public GetCustomersQuery(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }
}
