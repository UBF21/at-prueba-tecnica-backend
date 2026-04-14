using at_prueba_tecnica_backend.Api.Responses;
using Vali_Mediator.Core.Result;

namespace at_prueba_tecnica_backend.Api.Extensions;

/// <summary>
/// Extension methods to convert Vali-Mediator Result to API Response types.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Converts Result&lt;T&gt; to ApiResponse&lt;T&gt;.
    /// </summary>
    public static ApiResponse<T> ToApiResponse<T>(this Result<T> result)
    {
        return result.IsSuccess
            ? ApiResponse<T>.Ok(result.Value!)
            : ApiResponse<T>.Fail("An error occurred");
    }

    /// <summary>
    /// Converts Result&lt;List&lt;T&gt;&gt; to ListResponse&lt;T&gt;.
    /// </summary>
    public static ListResponse<T> ToListResponse<T>(this Result<List<T>> result)
    {
        return result.IsSuccess
            ? ListResponse<T>.Ok(result.Value!.AsReadOnly())
            : ListResponse<T>.Fail("An error occurred");
    }

    /// <summary>
    /// Converts Result&lt;(List&lt;T&gt;, int)&gt; (data + count) to PaginatedResponse&lt;T&gt;.
    /// Used when handler returns tuple with data and total count.
    /// </summary>
    public static PaginatedResponse<T> ToPaginatedResponse<T>(
        this Result<(List<T> Data, int Total)> result,
        int page,
        int pageSize)
    {
        if (result.IsSuccess)
        {
            var (data, total) = result.Value;
            return PaginatedResponse<T>.Ok(data.AsReadOnly(), page, pageSize, total);
        }

        return PaginatedResponse<T>.Fail("An error occurred");
    }
}
