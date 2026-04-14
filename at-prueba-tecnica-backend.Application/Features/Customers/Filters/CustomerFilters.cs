using at_prueba_tecnica_backend.Domain.Entities;
using Vali_Flow.Core.Builder;

namespace at_prueba_tecnica_backend.Application.Features.Customers.Filters;

/// <summary>
/// Reusable filters for Customer queries using Vali-Flow.
/// Builds dynamic LINQ expressions for customer lookup and filtering.
/// </summary>
public static class CustomerFilters
{
    /// <summary>
    /// Base filter: excludes deleted customers (soft delete).
    /// </summary>
    public static ValiFlowQuery<Customer> Active() =>
        new ValiFlowQuery<Customer>().IsNull(c => c.DeletedAt);

    /// <summary>
    /// Filter by customer ID.
    /// </summary>
    public static ValiFlowQuery<Customer> ById(Guid id) =>
        Active().And().EqualTo(c => c.Id, id);

    /// <summary>
    /// Filter by customer email.
    /// </summary>
    public static ValiFlowQuery<Customer> ByEmail(string email) =>
        Active().And().EqualTo(c => c.Email, email);

    /// <summary>
    /// Filter by customer name (contains).
    /// </summary>
    public static ValiFlowQuery<Customer> ByName(string name) =>
        Active().And().Contains(c => c.Name, name);
}
