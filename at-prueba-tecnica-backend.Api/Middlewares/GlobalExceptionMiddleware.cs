using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using at_prueba_tecnica_backend.Api.Responses;

namespace at_prueba_tecnica_backend.Api.Middlewares;

/// <summary>
/// Middleware global que captura excepciones no manejadas y retorna respuestas API consistentes.
/// Maneja específicamente DbUpdateException para extraer mensajes de error SQL.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        object response;

        // Handle DbUpdateException (database constraint violations, etc.)
        if (exception is DbUpdateException dbEx)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var message = ExtractDbErrorMessage(dbEx);

            response = new
            {
                success = false,
                data = (object?)null,
                message = message,
                errors = (object?)null
            };
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            response = new
            {
                success = false,
                data = (object?)null,
                message = "An error occurred while processing your request",
                errors = (object?)null
            };
        }

        return context.Response.WriteAsJsonAsync(response);
    }

    /// <summary>
    /// Extracts the most relevant error message from DbUpdateException.
    /// Looks for constraint violations and SQL-level error messages.
    /// </summary>
    private static string ExtractDbErrorMessage(DbUpdateException ex)
    {
        // Try to get the most specific error message
        var innerException = ex.InnerException;
        while (innerException?.InnerException != null)
        {
            innerException = innerException.InnerException;
        }

        if (innerException != null)
        {
            var message = innerException.Message;

            // Extract relevant parts from SQL Server error messages
            if (message.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
            {
                if (message.Contains("'Email'", StringComparison.OrdinalIgnoreCase))
                    return "El email ya está registrado";
                if (message.Contains("'OrderNumber'", StringComparison.OrdinalIgnoreCase))
                    return "El número de orden ya existe";
                return "El valor duplicado ya existe en la base de datos";
            }

            if (message.Contains("UNIQUE constraint failed", StringComparison.OrdinalIgnoreCase))
                return "El valor ingresado ya existe";

            return message;
        }

        return ex.Message;
    }
}
