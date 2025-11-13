namespace People.Domain.Persons.Events;

using People.Domain.Persons.ValueObjects;

/// <summary>
/// Domain event raised when a person's name is updated.
/// </summary>
public record PersonNameUpdatedEvent
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid Key { get; init; }

    /// <summary>
    /// Gets the previous name.
    /// </summary>
    public PersonName PreviousName { get; init; } = null!;

    /// <summary>
    /// Gets the new name.
    /// </summary>
    public PersonName NewName { get; init; } = null!;

    /// <summary>
    /// Initializes a new instance of <see cref="PersonNameUpdatedEvent"/>.
    /// </summary>
    /// <param name="key">The person's alternate key.</param>
    /// <param name="previousName">The previous name.</param>
    /// <param name="newName">The new name.</param>
    public PersonNameUpdatedEvent(Guid key, PersonName previousName, PersonName newName)
    {
        Key = key;
        PreviousName = previousName;
        NewName = newName;
    }
}

