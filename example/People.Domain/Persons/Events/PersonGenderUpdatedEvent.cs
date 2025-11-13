namespace People.Domain.Persons.Events;

using People.Domain.Persons.ValueObjects;

/// <summary>
/// Domain event raised when a person's gender is updated.
/// </summary>
public record PersonGenderUpdatedEvent
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid Key { get; init; }

    /// <summary>
    /// Gets the previous gender, if any.
    /// </summary>
    public Gender? PreviousGender { get; init; }

    /// <summary>
    /// Gets the new gender, if any.
    /// </summary>
    public Gender? NewGender { get; init; }

    /// <summary>
    /// Initializes a new instance of <see cref="PersonGenderUpdatedEvent"/>.
    /// </summary>
    /// <param name="key">The person's alternate key.</param>
    /// <param name="previousGender">The previous gender. Can be null.</param>
    /// <param name="newGender">The new gender. Can be null.</param>
    public PersonGenderUpdatedEvent(Guid key, Gender? previousGender, Gender? newGender)
    {
        Key = key;
        PreviousGender = previousGender;
        NewGender = newGender;
    }
}

