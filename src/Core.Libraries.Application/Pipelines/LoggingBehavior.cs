using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Diagnostics;
using Core.LibrariesApplication.Commands;

namespace Core.LibrariesApplication.Behavior;


public interface IAuditLog
{
    bool AuditEnable { get; }
}

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestType = typeof(TRequest).Name;

        LogContext.PushProperty("RequestType", requestType);

        if (request is IHasPermissionContext permissionContext)
        {
            var permission = permissionContext.GetPermission();
            LogContext.PushProperty("UserId", permission?.UserId ?? "undefined");
            LogContext.PushProperty("UserRoles", string.Join(",", permission?.Roles ?? Array.Empty<string>()));
        }

        if (request is IHasTraceContext traceContext)
        {
            var trace = traceContext.GetTrace();
            LogContext.PushProperty("CorrelationId", trace?.CorrelationId);
            LogContext.PushProperty("CausationId", trace?.CausationId);
            LogContext.PushProperty("TraceTimestamp", trace?.TimestampUtc);
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.LogInformation("Iniciando execução de {RequestType}: {@Request}", requestType, request);
            var response = await next();
            stopwatch.Stop();

            _logger.LogInformation("Finalizou {RequestType} em {Elapsed}ms. Resultado: {@Response}",
                requestType,
                stopwatch.ElapsedMilliseconds,
                response);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Erro ao executar {RequestType} em {Elapsed}ms.: {@Request}", requestType, stopwatch.ElapsedMilliseconds, request);
            throw;
        }
    }
}
