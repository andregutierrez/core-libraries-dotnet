namespace People.Domain.Contacts.Entities;

using Core.Libraries.Domain.Entities;
using People.Domain.Contacts.Events;
using People.Domain.Contacts.ValueObjects;

/// <summary>
/// Represents an email contact for a person.
/// </summary>
public class EmailContact : Contact
{
    /// <summary>
    /// Gets the email address.
    /// </summary>
    public Email Email { get; protected set; } = null!;

    /// <summary>
    /// Protected constructor for EF Core.
    /// </summary>
    protected EmailContact() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="EmailContact"/>.
    /// </summary>
    /// <param name="key">The alternate key for this contact.</param>
    /// <param name="personId">The person's entity ID this contact belongs to.</param>
    /// <param name="email">The email address.</param>
    /// <param name="isPrimary">Whether this is the primary email contact.</param>
    /// <param name="notes">Optional notes for this contact.</param>
    private EmailContact(AlternateKey key, EntityId personId, Email email, bool isPrimary = false, string? notes = null)
        : base(key, personId, ContactType.Email, isPrimary, notes)
    {
        ArgumentNullException.ThrowIfNull(email);
        Email = email;
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
    public static EmailContact Create(EntityId personId, Email email, bool isPrimary = false, string? notes = null)
    {
        ArgumentNullException.ThrowIfNull(personId);
        ArgumentNullException.ThrowIfNull(email);

        var contact = new EmailContact(AlternateKey.New(), personId, email, isPrimary, notes);
        contact.RegisterEvent(new ContactCreatedEvent(contact.Key.Value, personId.Value, ContactType.Email, email.Value));
        return contact;
    }

    /// <summary>
    /// Imports an email contact from an external system with a specific alternate key.
    /// </summary>
    /// <param name="key">The alternate key from the external system. Cannot be null.</param>
    /// <param name="personId">The person's entity ID this contact belongs to.</param>
    /// <param name="email">The email address.</param>
    /// <param name="isPrimary">Whether this is the primary email contact.</param>
    /// <param name="notes">Optional notes for this contact.</param>
    /// <returns>A new instance of <see cref="EmailContact"/> with the specified alternate key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key or email is null.</exception>
    public static EmailContact Import(AlternateKey key, EntityId personId, Email email, bool isPrimary = false, string? notes = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(personId);
        ArgumentNullException.ThrowIfNull(email);

        var contact = new EmailContact(key, personId, email, isPrimary, notes);
        contact.RegisterEvent(new ContactCreatedEvent(contact.Key.Value, personId.Value, ContactType.Email, email.Value));
        return contact;
    }

    /// <summary>
    /// Updates the email address for this contact.
    /// </summary>
    /// <param name="email">The new email address.</param>
    /// <exception cref="ArgumentNullException">Thrown when email is null.</exception>
    public void UpdateEmail(Email email)
    {
        ArgumentNullException.ThrowIfNull(email);

        var previousEmail = Email.Value;
        Email = email;

        RegisterEvent(new ContactUpdatedEvent(Key.Value, PersonId.Value, Type, previousEmail, email.Value));
    }

    /// <summary>
    /// Gets the contact value as a string (for display purposes).
    /// </summary>
    public override string DisplayValue => Email.Value;
}

