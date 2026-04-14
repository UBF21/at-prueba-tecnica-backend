using at_prueba_tecnica_backend.Api.Responses;
using Vali_Mediator.Core.Result;
using System.Collections.Generic;
using System.Reflection;

namespace at_prueba_tecnica_backend.Api.Extensions;

/// <summary>
/// Extension methods to convert Vali-Mediator Result to API Response types.
/// Uses reflection to access internal Result properties for error details.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Converts Result&lt;T&gt; to ApiResponse&lt;T&gt;.
    /// </summary>
    public static ApiResponse<T> ToApiResponse<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return ApiResponse<T>.Ok(result.Value!);

        var message = GetErrorMessage(result) ?? "An error occurred";
        var errors = GetValidationErrors(result);

        return ApiResponse<T>.Fail(message, errors);
    }

    /// <summary>
    /// Converts Result&lt;List&lt;T&gt;&gt; to ListResponse&lt;T&gt;.
    /// </summary>
    public static ListResponse<T> ToListResponse<T>(this Result<List<T>> result)
    {
        if (result.IsSuccess)
            return ListResponse<T>.Ok(result.Value!.AsReadOnly());

        var message = GetErrorMessage(result) ?? "An error occurred";
        var errors = GetValidationErrors(result);

        return ListResponse<T>.Fail(message, errors);
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

        var message = GetErrorMessage(result) ?? "An error occurred";
        var errors = GetValidationErrors(result);

        return PaginatedResponse<T>.Fail(message, errors);
    }

    /// <summary>
    /// Extracts error message from Result using reflection.
    /// Tries multiple property names to find the error message.
    /// </summary>
    private static string? GetErrorMessage(object result)
    {
        var type = result.GetType();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

        foreach (var propName in new[] { "Error", "Message", "ErrorMessage" })
        {
            var prop = properties.FirstOrDefault(p =>
                p.Name.Equals(propName, StringComparison.OrdinalIgnoreCase));
            if (prop?.GetValue(result) is string message && !string.IsNullOrEmpty(message))
                return message;
        }

        return null;
    }

    /// <summary>
    /// Extracts validation errors from Result using reflection.
    /// Returns a dictionary of field-level errors from Vali-Validation.
    /// </summary>
    private static object? GetValidationErrors(object result)
    {
        var type = result.GetType();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);

        // Try to find ValidationErrors or Errors property
        var errorsProp = properties.FirstOrDefault(p =>
            p.Name.Equals("ValidationErrors", StringComparison.OrdinalIgnoreCase));

        if (errorsProp == null)
        {
            errorsProp = properties.FirstOrDefault(p =>
                p.Name.Equals("Errors", StringComparison.OrdinalIgnoreCase));
        }

        if (errorsProp != null)
        {
            var errorsValue = errorsProp.GetValue(result);
            if (errorsValue is not null)
            {
                var dictType = errorsValue.GetType();
                if (dictType.IsGenericType &&
                    (dictType.GetGenericTypeDefinition().Name.StartsWith("IDictionary") ||
                     dictType.GetGenericTypeDefinition().Name.StartsWith("Dictionary")))
                {
                    return errorsValue;
                }
            }
        }

        return null;
    }
}
