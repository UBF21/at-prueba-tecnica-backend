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
        if (result.IsSuccess)
            return ApiResponse<T>.Ok(result.Data!);

        var errors = result.Errors?.Select(e => e.Message).ToList();
        return ApiResponse<T>.Fail(result.Message ?? "An error occurred", errors);
    }

    /// <summary>
    /// Converts Result&lt;List&lt;T&gt;&gt; to ListResponse&lt;T&gt;.
    /// </summary>
    public static ListResponse<T> ToListResponse<T>(this Result<List<T>> result)
    {
        if (result.IsSuccess)
            return ListResponse<T>.Ok(result.Data!.AsReadOnly());

        var errors = result.Errors?.Select(e => e.Message).ToList();
        return ListResponse<T>.Fail(result.Message ?? "An error occurred", errors);
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
            var (data, total) = result.Data;
            return PaginatedResponse<T>.Ok(data.AsReadOnly(), page, pageSize, total);
        }

        var errors = result.Errors?.Select(e => e.Message).ToList();
        return PaginatedResponse<T>.Fail(result.Message ?? "An error occurred", errors);
    }
}
