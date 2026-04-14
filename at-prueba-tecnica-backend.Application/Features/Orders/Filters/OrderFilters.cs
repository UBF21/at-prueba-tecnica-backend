using at_prueba_tecnica_backend.Domain.Entities;
using Vali_Flow.Core.Builder;

namespace at_prueba_tecnica_backend.Application.Features.Orders.Filters;

/// <summary>
/// Reusable filters for Order queries using Vali-Flow.
/// Builds dynamic LINQ expressions without external dependencies.
/// </summary>
public static class OrderFilters
{
    /// <summary>
    /// Base filter: excludes deleted orders (soft delete).
    /// </summary>
    public static ValiFlowQuery<Order> Active() =>
        new ValiFlowQuery<Order>().IsNull(o => o.DeletedAt);

    /// <summary>
    /// Filter by status (conditional at build time).
    /// If status is null or empty, returns only the Active filter.
    /// </summary>
    public static ValiFlowQuery<Order> ActiveByStatus(string? status)
    {
        var filter = Active();
        if (!string.IsNullOrEmpty(status))
            filter = filter.And().EqualTo(o => o.Status.ToString(), status);
        return filter;
    }

    /// <summary>
    /// Filter by order ID.
    /// </summary>
    public static ValiFlowQuery<Order> ById(int id) =>
        Active().And().EqualTo(o => o.Id, id);

    /// <summary>
    /// Filter by order number (for validating uniqueness).
    /// </summary>
    public static ValiFlowQuery<Order> ByOrderNumber(string orderNumber) =>
        new ValiFlowQuery<Order>().EqualTo(o => o.OrderNumber, orderNumber);

    /// <summary>
    /// Filter by customer ID.
    /// </summary>
    public static ValiFlowQuery<Order> ByCustomerId(int customerId) =>
        Active().And().EqualTo(o => o.CustomerId, customerId);
}
