namespace People.Domain.Contacts.Entities;

using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using People.Domain.Contacts.Events;
using People.Domain.Contacts.ValueObjects;

/// <summary>
/// Base class representing contact information for a person.
/// This is an abstract class that should be inherited by specific contact types.
/// </summary>
public abstract class Contact : Aggregate<EntityId>, IHasAlternateKey
{
    /// <summary>
    /// Gets the alternate key for this contact.
    /// Used for external references and cross-system integration.
    /// </summary>
    public AlternateKey Key { get; protected set; } = null!;

    /// <summary>
    /// Gets the person's entity ID this contact belongs to.
    /// </summary>
    public EntityId PersonId { get; protected set; }

    /// <summary>
    /// Gets the type of contact information.
    /// </summary>
    public ContactType Type { get; protected set; } = null!;

    /// <summary>
    /// Gets the contact value as a string (for display purposes).
    /// Must be implemented by derived classes.
    /// </summary>
    public abstract string DisplayValue { get; }

    /// <summary>
    /// Gets a value indicating whether this is the primary contact of its type.
    /// </summary>
    public bool IsPrimary { get; protected set; }

    /// <summary>
    /// Gets additional notes or context for this contact.
    /// </summary>
    public string? Notes { get; protected set; }

    /// <summary>
    /// Protected constructor for EF Core.
    /// </summary>
    protected Contact()
    {
        Key = default!;
        PersonId = default!;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Contact"/>.
    /// This constructor is protected to allow inheritance.
    /// </summary>
    /// <param name="key">The alternate key for this contact.</param>
    /// <param name="personId">The person's entity ID this contact belongs to.</param>
    /// <param name="type">The type of contact information.</param>
    /// <param name="isPrimary">Whether this is the primary contact of its type.</param>
    /// <param name="notes">Optional notes for this contact.</param>
    /// <exception cref="ArgumentNullException">Thrown when key, personId, or type is null.</exception>
    protected Contact(AlternateKey key, EntityId personId, ContactType type, bool isPrimary = false, string? notes = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(personId);
        ArgumentNullException.ThrowIfNull(type);

        Key = key;
        PersonId = personId;
        Type = type;
        IsPrimary = isPrimary;
        Notes = notes;
    }

    /// <summary>
    /// Creates a new email contact.
    /// </summary>
    /// <param name="personId">The person's entity ID this contact belongs to.</param>
    /// <param name="email">The email address.</param>
    /// <param name="isPrimary">Whether this is the primary email contact.</param>
    /// <param name="notes">Optional notes for this contact.</param>
    /// <returns>A new instance of <see cref="EmailContact"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when email is null.</exception>
    public static EmailContact CreateEmail(EntityId personId, Email email, bool isPrimary = false, string? notes = null)
        => EmailContact.Create(personId, email, isPrimary, notes);

    /// <summary>
    /// Creates a new phone contact.
    /// </summary>
    /// <param name="personId">The person's entity ID this contact belongs to.</param>
    /// <param name="phone">The phone number.</param>
    /// <param name="type">The type of phone contact (Phone, Mobile, or WhatsApp).</param>
    /// <param name="isPrimary">Whether this is the primary phone contact.</param>
    /// <param name="notes">Optional notes for this contact.</param>
    /// <returns>A new instance of <see cref="PhoneContact"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when phone or type is null.</exception>
    /// <exception cref="ArgumentException">Thrown when type is not a phone type.</exception>
    public static PhoneContact CreatePhone(EntityId personId, PhoneNumber phone, ContactType type, bool isPrimary = false, string? notes = null)
        => PhoneContact.Create(personId, phone, type, isPrimary, notes);

    /// <summary>
    /// Creates a new social media contact.
    /// </summary>
    /// <param name="personId">The person's entity ID this contact belongs to.</param>
    /// <param name="socialMedia">The social media account.</param>
    /// <param name="isPrimary">Whether this is the primary social media contact.</param>
    /// <param name="notes">Optional notes for this contact.</param>
    /// <returns>A new instance of <see cref="SocialMediaContact"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when socialMedia is null.</exception>
    public static SocialMediaContact CreateSocialMedia(EntityId personId, SocialMediaAccount socialMedia, bool isPrimary = false, string? notes = null)
        => SocialMediaContact.Create(personId, socialMedia, isPrimary, notes);

    /// <summary>
    /// Imports a contact from an external system with a specific alternate key.
    /// </summary>
    /// <param name="key">The alternate key from the external system. Cannot be null.</param>
    /// <param name="personId">The person's entity ID this contact belongs to.</param>
    /// <param name="type">The type of contact information.</param>
    /// <param name="email">The email address, if this is an email contact.</param>
    /// <param name="phone">The phone number, if this is a phone contact.</param>
    /// <param name="socialMedia">The social media account, if this is a social media contact.</param>
    /// <param name="isPrimary">Whether this is the primary contact of its type.</param>
    /// <param name="notes">Optional notes for this contact.</param>
    /// <returns>A new instance of <see cref="Contact"/> with the specified alternate key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key, personId, or type is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the contact value doesn't match the type.</exception>
    public static Contact Import(AlternateKey key, EntityId personId, ContactType type, Email? email = null, PhoneNumber? phone = null, SocialMediaAccount? socialMedia = null, bool isPrimary = false, string? notes = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(personId);
        ArgumentNullException.ThrowIfNull(type);

        return type.Code switch
        {
            1 => EmailContact.Import(key, personId, email ?? throw new ArgumentException("Email must be provided for Email contact type.", nameof(email)), isPrimary, notes),
            2 or 3 or 4 => PhoneContact.Import(key, personId, type, phone ?? throw new ArgumentException("Phone must be provided for Phone contact types.", nameof(phone)), isPrimary, notes),
            5 => SocialMediaContact.Import(key, personId, socialMedia ?? throw new ArgumentException("SocialMedia must be provided for Social Media contact type.", nameof(socialMedia)), isPrimary, notes),
            _ => throw new ArgumentException($"Unknown contact type: {type.Name}", nameof(type))
        };
    }

    /// <summary>
    /// Sets this contact as the primary contact of its type.
    /// </summary>
    public void SetAsPrimary()
    {
        if (IsPrimary)
            return;

        IsPrimary = true;
        RegisterEvent(new ContactSetAsPrimaryEvent(Key.Value, PersonId.Value, Type));
    }

    /// <summary>
    /// Removes the primary flag from this contact.
    /// </summary>
    public void RemovePrimary()
    {
        if (!IsPrimary)
            return;

        IsPrimary = false;
        RegisterEvent(new ContactRemovedAsPrimaryEvent(Key.Value, PersonId.Value, Type));
    }


    /// <summary>
    /// Updates the notes for this contact.
    /// </summary>
    /// <param name="notes">The new notes. Can be null to remove notes.</param>
    public void UpdateNotes(string? notes)
    {
        var previousNotes = Notes;
        Notes = notes;

        RegisterEvent(new ContactNotesUpdatedEvent(Key.Value, PersonId.Value, previousNotes, notes));
    }
}

