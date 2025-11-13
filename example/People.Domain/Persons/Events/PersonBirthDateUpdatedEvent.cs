namespace People.Domain.Persons.Events;

using People.Domain.Persons.ValueObjects;

/// <summary>
/// Domain event raised when a person's birth date is updated.
/// </summary>
public record PersonBirthDateUpdatedEvent
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid Key { get; init; }

    /// <summary>
    /// Gets the previous birth date, if any.
    /// </summary>
    public BirthDate? PreviousBirthDate { get; init; }

    /// <summary>
    /// Gets the new birth date, if any.
    /// </summary>
    public BirthDate? NewBirthDate { get; init; }

    /// <summary>
    /// Initializes a new instance of <see cref="PersonBirthDateUpdatedEvent"/>.
    /// </summary>
    /// <param name="key">The person's alternate key.</param>
    /// <param name="previousBirthDate">The previous birth date. Can be null.</param>
    /// <param name="newBirthDate">The new birth date. Can be null.</param>
    public PersonBirthDateUpdatedEvent(Guid key, BirthDate? previousBirthDate, BirthDate? newBirthDate)
    {
        Key = key;
        PreviousBirthDate = previousBirthDate;
        NewBirthDate = newBirthDate;
    }
}

