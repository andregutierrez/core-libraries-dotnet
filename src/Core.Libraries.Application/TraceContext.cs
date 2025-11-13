namespace Core.Libraries.Application;

public record TraceContext(
    Guid CorrelationId,
    Guid CausationId,
    DateTime TimestampUtc
);
