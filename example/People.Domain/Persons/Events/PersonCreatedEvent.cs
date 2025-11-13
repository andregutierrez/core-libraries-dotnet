namespace People.Domain.Persons.Events;

using Core.Libraries.Domain.Entities;
using People.Domain.Persons.ValueObjects;

/// <summary>
/// Domain event raised when a new person is created.
/// </summary>
public record PersonCreatedEvent
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid Key { get; init; }

    /// <summary>
    /// Gets the person's name.
    /// </summary>
    public PersonName Name { get; init; } = null!;

    /// <summary>
    /// Gets the person's birth date, if any.
    /// </summary>
    public BirthDate? BirthDate { get; init; }

    /// <summary>
    /// Gets the person's gender, if any.
    /// </summary>
    public Gender? Gender { get; init; }

    /// <summary>
    /// Initializes a new instance of <see cref="PersonCreatedEvent"/>.
    /// </summary>
    /// <param name="key">The person's alternate key.</param>
    /// <param name="name">The person's name.</param>
    /// <param name="birthDate">The person's birth date. Optional.</param>
    /// <param name="gender">The person's gender. Optional.</param>
    public PersonCreatedEvent(Guid key, PersonName name, BirthDate? birthDate = null, Gender? gender = null)
    {
        Key = key;
        Name = name;
        BirthDate = birthDate;
        Gender = gender;
    }
}

