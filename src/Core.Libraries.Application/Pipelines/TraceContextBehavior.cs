using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using Core.Libraries.Application.Commands;

namespace Core.Libraries.Application.Pipelines;

public class TraceContextBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TraceContextBehavior(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is BaseCommand baseCommand)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var correlationId = GetCorrelationId(httpContext) ?? Guid.NewGuid(); // fallback
            var causationId = Guid.NewGuid(); // novo ID para a operação atual
            var timestamp = DateTime.UtcNow;

            var trace = new TraceContext(correlationId, causationId, timestamp);

            baseCommand.SetTrace(trace);

            LogContext.PushProperty("CorrelationId", correlationId);
            LogContext.PushProperty("CausationId", causationId);
            LogContext.PushProperty("TraceTimestamp", timestamp);
        }

        return await next();
    }

    private Guid? GetCorrelationId(HttpContext? context)
    {
        if (context?.Request.Headers.TryGetValue("X-Correlation-Id", out var header) == true &&
            Guid.TryParse(header.ToString(), out var guid))
        {
            return guid;
        }

        return null;
    }
}
