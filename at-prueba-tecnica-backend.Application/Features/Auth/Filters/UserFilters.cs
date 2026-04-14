using at_prueba_tecnica_backend.Domain.Entities;
using Vali_Flow.Core.Builder;

namespace at_prueba_tecnica_backend.Application.Features.Auth.Filters;

/// <summary>
/// Reusable filters for User queries using Vali-Flow.
/// Builds dynamic LINQ expressions for user authentication and lookup.
/// </summary>
public static class UserFilters
{
    /// <summary>
    /// Base filter: excludes deleted users (soft delete).
    /// </summary>
    public static ValiFlowQuery<User> Active() =>
        new ValiFlowQuery<User>().IsNull(u => u.DeletedAt);

    /// <summary>
    /// Filter by user email.
    /// </summary>
    public static ValiFlowQuery<User> ByEmail(string email) =>
        Active().And().EqualTo(u => u.Email, email);

    /// <summary>
    /// Filter by user ID.
    /// </summary>
    public static ValiFlowQuery<User> ById(int id) =>
        Active().And().EqualTo(u => u.Id, id);
}
