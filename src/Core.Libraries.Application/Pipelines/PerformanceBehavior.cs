using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Core.LibrariesApplication.Behavior;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
    private const int WarningThresholdMilliseconds = 500;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await next();
        stopwatch.Stop();

        var elapsedMs = stopwatch.ElapsedMilliseconds;
        if (elapsedMs > WarningThresholdMilliseconds)
        {
            _logger.LogWarning("Performance: {RequestType} demorou {Elapsed}ms. Payload: {@Request}",
                typeof(TRequest).Name,
                elapsedMs,
                request);
        }
        else
        {
            _logger.LogDebug("Performance: {RequestType} executado em {Elapsed}ms",
                typeof(TRequest).Name,
                elapsedMs);
        }

        return response;
    }
}
