namespace at_prueba_tecnica_backend.Api.Responses;

/// <summary>
/// Generic API response wrapper for list results (non-paginated).
/// Supports both IReadOnlyList&lt;string&gt; and IReadOnlyDictionary&lt;string, IReadOnlyList&lt;string&gt;&gt; for validation errors.
/// </summary>
/// <typeparam name="T">Type of items in the data collection</typeparam>
public record ListResponse<T>(
    bool Success,
    IReadOnlyList<T> Data,
    string? Message = null,
    object? Errors = null)
{
    /// <summary>
    /// Creates a successful list response.
    /// </summary>
    public static ListResponse<T> Ok(IReadOnlyList<T> data, string? message = null)
        => new(Success: true, Data: data, Message: message);

    /// <summary>
    /// Creates a failed response with error message.
    /// </summary>
    public static ListResponse<T> Fail(string message, object? errors = null)
        => new(Success: false, Data: new List<T>(), Message: message, Errors: errors);

    /// <summary>
    /// Creates a failed response with multiple errors.
    /// </summary>
    public static ListResponse<T> Fail(IReadOnlyList<string> errors)
        => new(Success: false, Data: new List<T>(), Errors: errors);
}
