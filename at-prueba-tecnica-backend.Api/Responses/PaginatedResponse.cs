namespace at_prueba_tecnica_backend.Api.Responses;

/// <summary>
/// Generic API response wrapper for paginated list results.
/// Supports both IReadOnlyList&lt;string&gt; and IReadOnlyDictionary&lt;string, IReadOnlyList&lt;string&gt;&gt; for validation errors.
/// </summary>
/// <typeparam name="T">Type of items in the data collection</typeparam>
public record PaginatedResponse<T>(
    bool Success,
    IReadOnlyList<T> Data,
    int Page,
    int PageSize,
    int Total,
    int TotalPages,
    string? Message = null,
    object? Errors = null)
{
    /// <summary>
    /// Creates a successful paginated response.
    /// </summary>
    public static PaginatedResponse<T> Ok(
        IReadOnlyList<T> data,
        int page,
        int pageSize,
        int total,
        string? message = null)
    {
        var totalPages = (int)Math.Ceiling((double)total / pageSize);
        return new(Success: true, Data: data, Page: page, PageSize: pageSize, Total: total, TotalPages: totalPages, Message: message);
    }

    /// <summary>
    /// Creates a failed paginated response with error message.
    /// </summary>
    public static PaginatedResponse<T> Fail(string message, object? errors = null)
        => new(Success: false, Data: new List<T>(), Page: 0, PageSize: 0, Total: 0, TotalPages: 0, Message: message, Errors: errors);

    /// <summary>
    /// Creates a failed paginated response with multiple errors.
    /// </summary>
    public static PaginatedResponse<T> Fail(IReadOnlyList<string> errors)
        => new(Success: false, Data: new List<T>(), Page: 0, PageSize: 0, Total: 0, TotalPages: 0, Errors: errors);
}
