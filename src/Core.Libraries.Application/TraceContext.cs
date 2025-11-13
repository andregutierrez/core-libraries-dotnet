namespace Core.LibrariesApplication;

public record TraceContext(
    Guid CorrelationId,
    Guid CausationId,
    DateTime TimestampUtc
);
