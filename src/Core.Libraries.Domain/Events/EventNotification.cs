using MediatR;

namespace Core.Libraries.Domain.Events;

/// <summary>
/// Represents a base event notification that implements <see cref="INotification"/>.
/// Intended for use with MediatR to handle domain or integration events.
/// </summary>
public abstract class EventNotification : INotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventNotification"/> class.
    /// </summary>
    /// <param name="key">The unique identifier of the event.</param>
    /// <param name="occurredAt">The UTC timestamp indicating when the event occurred.</param>
    protected EventNotification()
    {
        Key = Guid.NewGuid();
        OccurredAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventNotification"/> class.
    /// </summary>
    /// <param name="key">The unique identifier of the event.</param>
    /// <param name="occurredAt">The UTC timestamp indicating when the event occurred.</param>
    protected EventNotification(Guid key, DateTimeOffset occurredAt)
    {
        Key = key;
        OccurredAt = occurredAt;
    }

    /// <summary>
    /// Gets the unique identifier of the event.
    /// </summary>
    public Guid Key { get; init; }

    /// <summary>
    /// Gets the UTC timestamp of when the event occurred.
    /// </summary>
    public DateTimeOffset OccurredAt { get; init; }
}
