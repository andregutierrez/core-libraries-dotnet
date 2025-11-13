namespace People.Domain.Contacts.Events;

using People.Domain.Contacts.ValueObjects;

/// <summary>
/// Domain event raised when a contact is set as primary.
/// </summary>
public record ContactSetAsPrimaryEvent
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
    /// Initializes a new instance of <see cref="ContactSetAsPrimaryEvent"/>.
    /// </summary>
    /// <param name="contactKey">The contact's alternate key.</param>
    /// <param name="personId">The person's entity ID.</param>
    /// <param name="type">The type of contact information.</param>
    public ContactSetAsPrimaryEvent(Guid contactKey, int personId, ContactType type)
    {
        ContactKey = contactKey;
        PersonId = personId;
        Type = type;
    }
}

