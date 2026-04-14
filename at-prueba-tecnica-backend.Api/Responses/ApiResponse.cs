namespace at_prueba_tecnica_backend.Api.Responses;

/// <summary>
/// Generic API response wrapper for single objects.
/// </summary>
/// <typeparam name="T">Type of the response data</typeparam>
public record ApiResponse<T>(
    bool Success,
    T? Data,
    string? Message = null,
    IReadOnlyList<string>? Errors = null)
{
    /// <summary>
    /// Creates a successful response with data.
    /// </summary>
    public static ApiResponse<T> Ok(T data, string? message = null)
        => new(Success: true, Data: data, Message: message);

    /// <summary>
    /// Creates a failed response with error message.
    /// </summary>
    public static ApiResponse<T> Fail(string message, IReadOnlyList<string>? errors = null)
        => new(Success: false, Data: default, Message: message, Errors: errors);

    /// <summary>
    /// Creates a failed response with multiple errors.
    /// </summary>
    public static ApiResponse<T> Fail(IReadOnlyList<string> errors)
        => new(Success: false, Data: default, Errors: errors);
}
