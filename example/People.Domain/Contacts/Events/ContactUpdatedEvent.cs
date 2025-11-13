namespace People.Domain.Contacts.Events;

using People.Domain.Contacts.ValueObjects;

/// <summary>
/// Domain event raised when a contact's value is updated.
/// </summary>
public record ContactUpdatedEvent
{
    /// <summary>
    /// Gets the contact's alternate key.
    /// </summary>
    public Guid ContactKey { get; init; }

    /// <summary>
    /// Gets the person's entity ID this contact belongs to.
    /// </summary>
    public int PersonId { get; init; }

    /// <summary>
    /// Gets the type of contact information.
    /// </summary>
    public ContactType Type { get; init; } = null!;

    /// <summary>
    /// Gets the previous contact value.
    /// </summary>
    public string PreviousValue { get; init; } = string.Empty;

    /// <summary>
    /// Gets the new contact value.
    /// </summary>
    public string NewValue { get; init; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of <see cref="ContactUpdatedEvent"/>.
    /// </summary>
    /// <param name="contactKey">The contact's alternate key.</param>
    /// <param name="personId">The person's entity ID.</param>
    /// <param name="type">The type of contact information.</param>
    /// <param name="previousValue">The previous contact value.</param>
    /// <param name="newValue">The new contact value.</param>
    public ContactUpdatedEvent(Guid contactKey, int personId, ContactType type, string previousValue, string newValue)
    {
        ContactKey = contactKey;
        PersonId = personId;
        Type = type;
        PreviousValue = previousValue;
        NewValue = newValue;
    }
}

