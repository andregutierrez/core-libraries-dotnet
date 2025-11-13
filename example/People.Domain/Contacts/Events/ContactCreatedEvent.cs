namespace People.Domain.Contacts.Events;

using People.Domain.Contacts.ValueObjects;

/// <summary>
/// Domain event raised when a new contact is created.
/// </summary>
public record ContactCreatedEvent
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
    /// Gets the contact value (email address or phone number).
    /// </summary>
    public string Value { get; init; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of <see cref="ContactCreatedEvent"/>.
    /// </summary>
    /// <param name="contactKey">The contact's alternate key.</param>
    /// <param name="personId">The person's entity ID.</param>
    /// <param name="type">The type of contact information.</param>
    /// <param name="value">The contact value.</param>
    public ContactCreatedEvent(Guid contactKey, int personId, ContactType type, string value)
    {
        ContactKey = contactKey;
        PersonId = personId;
        Type = type;
        Value = value;
    }
}

