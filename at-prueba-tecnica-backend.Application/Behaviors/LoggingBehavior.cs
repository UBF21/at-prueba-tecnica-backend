using Microsoft.Extensions.Logging;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Behaviors;

/// <summary>
/// Comportamiento del pipeline de Mediator que registra (logs) el inicio y fin
/// de cada request/command/query.
/// </summary>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, Func<Task<TResponse>> next, CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("[START] Procesando {RequestName}", requestName);

        try
        {
            var response = await next();
            _logger.LogInformation("[END] {RequestName} completado exitosamente", requestName);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ERROR] {RequestName} falló con excepción", requestName);
            throw;
        }
    }
}
