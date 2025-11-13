namespace People.Domain.Contacts.Events;

/// <summary>
/// Domain event raised when a contact's notes are updated.
/// </summary>
public record ContactNotesUpdatedEvent
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
    /// Gets the previous notes, if any.
    /// </summary>
    public string? PreviousNotes { get; init; }

    /// <summary>
    /// Gets the new notes, if any.
    /// </summary>
    public string? NewNotes { get; init; }

    /// <summary>
    /// Initializes a new instance of <see cref="ContactNotesUpdatedEvent"/>.
    /// </summary>
    /// <param name="contactKey">The contact's alternate key.</param>
    /// <param name="personId">The person's entity ID.</param>
    /// <param name="previousNotes">The previous notes. Can be null.</param>
    /// <param name="newNotes">The new notes. Can be null.</param>
    public ContactNotesUpdatedEvent(Guid contactKey, int personId, string? previousNotes, string? newNotes)
    {
        ContactKey = contactKey;
        PersonId = personId;
        PreviousNotes = previousNotes;
        NewNotes = newNotes;
    }
}

