namespace People.Domain.Persons.Events;

using People.Domain.Persons.Statuses;

/// <summary>
/// Domain event raised when a person's status is changed.
/// </summary>
public record PersonStatusChangedEvent
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets the previous status type, if any.
    /// </summary>
    public PersonStatusType? PreviousStatus { get; init; }

    /// <summary>
    /// Gets the new status type.
    /// </summary>
    public PersonStatusType NewStatus { get; init; }

    /// <summary>
    /// Gets the notes associated with the status change, if any.
    /// </summary>
    public string Notes { get; init; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of <see cref="PersonStatusChangedEvent"/>.
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="previousStatus">The previous status type. Can be null.</param>
    /// <param name="newStatus">The new status type.</param>
    /// <param name="notes">Optional notes associated with the status change.</param>
    public PersonStatusChangedEvent(Guid personKey, PersonStatusType? previousStatus, PersonStatusType newStatus, string notes = "")
    {
        PersonKey = personKey;
        PreviousStatus = previousStatus;
        NewStatus = newStatus;
        Notes = notes;
    }
}

