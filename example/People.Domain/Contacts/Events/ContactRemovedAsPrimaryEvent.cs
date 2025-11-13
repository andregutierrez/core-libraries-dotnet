namespace People.Domain.Contacts.Events;

using People.Domain.Contacts.ValueObjects;

/// <summary>
/// Domain event raised when a contact is removed as primary.
/// </summary>
public record ContactRemovedAsPrimaryEvent
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
    /// Initializes a new instance of <see cref="ContactRemovedAsPrimaryEvent"/>.
    /// </summary>
    /// <param name="contactKey">The contact's alternate key.</param>
    /// <param name="personId">The person's entity ID.</param>
    /// <param name="type">The type of contact information.</param>
    public ContactRemovedAsPrimaryEvent(Guid contactKey, int personId, ContactType type)
    {
        ContactKey = contactKey;
        PersonId = personId;
        Type = type;
    }
}

