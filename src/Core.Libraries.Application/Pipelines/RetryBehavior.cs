using MediatR;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Core.Libraries.Application.Pipelines;

public class RetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<RetryBehavior<TRequest, TResponse>> _logger;

    public RetryBehavior(ILogger<RetryBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Define a política de retry com até 3 tentativas e delays exponenciais
        AsyncRetryPolicy retryPolicy = Policy
            .Handle<TimeoutException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(200 * Math.Pow(2, attempt)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(exception,
                        "Retry {RetryCount} for {RequestName} after {Delay}ms due to: {Exception}",
                        retryCount, typeof(TRequest).Name, timeSpan.TotalMilliseconds, exception.GetType().Name);
                });

        return await retryPolicy.ExecuteAsync(() => next());
    }
}
