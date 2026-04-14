using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;

namespace at_prueba_tecnica_backend.Application.Features.Customers.Commands;

/// <summary>
/// Command to update an existing customer.
/// </summary>
public class UpdateCustomerCommand : IRequest<Result<UpdateCustomerCommand.Response>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }

    public class Response
    {
        public int Code { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
