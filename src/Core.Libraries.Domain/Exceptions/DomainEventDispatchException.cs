namespace Core.Libraries.Domain.Exceptions;

/// <summary>
/// Represents a base exception thrown when a domain event fails to be dispatched to its handler(s) or transport.
/// </summary>
/// <remarks>
/// This exception is typically used when an event is raised successfully, but fails during propagation,
/// such as through in-memory handlers, event buses, or external integrations.
/// </remarks>
public abstract class DomainEventDispatchException : DomainException
{
    /// <summary>
    /// Gets the type or name of the event that failed to dispatch.
    /// </summary>
    public string EventName { get; }

    /// <summary>
    /// Gets the identifier of the aggregate or context that produced the event.
    /// </summary>
    public string? SourceId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainEventDispatchException"/> class.
    /// </summary>
    /// <param name="eventName">The name of the domain event that failed to dispatch.</param>
    /// <param name="domainContext">The aggregate or logical context that raised the event.</param>
    /// <param name="sourceId">The identifier of the aggregate or entity (optional).</param>
    /// <param name="message">A descriptive message for the error.</param>
    /// <param name="details">Optional structured data for diagnostics.</param>
    protected DomainEventDispatchException(
        string eventName,
        string domainContext,
        string? sourceId,
        string message,
        object? details = null)
        : base(
            message: message,
            errorCode: "DOMAIN_EVENT_DISPATCH_FAILED",
            domainContext: domainContext,
            details: details ?? new { Event = eventName, SourceId = sourceId })
    {
        EventName = eventName;
        SourceId = sourceId;
    }
}
