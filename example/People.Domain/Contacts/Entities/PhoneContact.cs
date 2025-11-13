namespace People.Domain.Contacts.Entities;

using Core.Libraries.Domain.Entities;
using People.Domain.Contacts.Events;
using People.Domain.Contacts.ValueObjects;

/// <summary>
/// Represents a phone contact for a person (Phone, Mobile, or WhatsApp).
/// </summary>
public class PhoneContact : Contact
{
    /// <summary>
    /// Gets the phone number.
    /// </summary>
    public PhoneNumber Phone { get; protected set; } = null!;

    /// <summary>
    /// Protected constructor for EF Core.
    /// </summary>
    protected PhoneContact() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="PhoneContact"/>.
    /// </summary>
    /// <param name="key">The alternate key for this contact.</param>
    /// <param name="personId">The person's entity ID this contact belongs to.</param>
    /// <param name="type">The type of phone contact (Phone, Mobile, or WhatsApp).</param>
    /// <param name="phone">The phone number.</param>
    /// <param name="isPrimary">Whether this is the primary phone contact.</param>
    /// <param name="notes">Optional notes for this contact.</param>
    private PhoneContact(AlternateKey key, EntityId personId, ContactType type, PhoneNumber phone, bool isPrimary = false, string? notes = null)
        : base(key, personId, type, isPrimary, notes)
    {
        ArgumentNullException.ThrowIfNull(phone);

        if (type.Code != ContactType.Phone.Code && type.Code != ContactType.Mobile.Code && type.Code != ContactType.WhatsApp.Code)
            throw new ArgumentException("Type must be Phone, Mobile, or WhatsApp.", nameof(type));

        Phone = phone;
    }

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
    public static PhoneContact Create(EntityId personId, PhoneNumber phone, ContactType type, bool isPrimary = false, string? notes = null)
    {
        ArgumentNullException.ThrowIfNull(personId);
        ArgumentNullException.ThrowIfNull(phone);
        ArgumentNullException.ThrowIfNull(type);

        if (type.Code != ContactType.Phone.Code && type.Code != ContactType.Mobile.Code && type.Code != ContactType.WhatsApp.Code)
            throw new ArgumentException("Type must be Phone, Mobile, or WhatsApp.", nameof(type));

        var contact = new PhoneContact(AlternateKey.New(), personId, type, phone, isPrimary, notes);
        contact.RegisterEvent(new ContactCreatedEvent(contact.Key.Value, personId.Value, type, phone.Formatted));
        return contact;
    }

    /// <summary>
    /// Imports a phone contact from an external system with a specific alternate key.
    /// </summary>
    /// <param name="key">The alternate key from the external system. Cannot be null.</param>
    /// <param name="personId">The person's entity ID this contact belongs to.</param>
    /// <param name="type">The type of phone contact (Phone, Mobile, or WhatsApp).</param>
    /// <param name="phone">The phone number.</param>
    /// <param name="isPrimary">Whether this is the primary phone contact.</param>
    /// <param name="notes">Optional notes for this contact.</param>
    /// <returns>A new instance of <see cref="PhoneContact"/> with the specified alternate key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key, phone, or type is null.</exception>
    /// <exception cref="ArgumentException">Thrown when type is not a phone type.</exception>
    public static PhoneContact Import(AlternateKey key, EntityId personId, ContactType type, PhoneNumber phone, bool isPrimary = false, string? notes = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(personId);
        ArgumentNullException.ThrowIfNull(phone);
        ArgumentNullException.ThrowIfNull(type);

        if (type.Code != ContactType.Phone.Code && type.Code != ContactType.Mobile.Code && type.Code != ContactType.WhatsApp.Code)
            throw new ArgumentException("Type must be Phone, Mobile, or WhatsApp.", nameof(type));

        var contact = new PhoneContact(key, personId, type, phone, isPrimary, notes);
        contact.RegisterEvent(new ContactCreatedEvent(contact.Key.Value, personId.Value, type, phone.Formatted));
        return contact;
    }

    /// <summary>
    /// Updates the phone number for this contact.
    /// </summary>
    /// <param name="phone">The new phone number.</param>
    /// <exception cref="ArgumentNullException">Thrown when phone is null.</exception>
    public void UpdatePhone(PhoneNumber phone)
    {
        ArgumentNullException.ThrowIfNull(phone);

        var previousPhone = Phone.Formatted;
        Phone = phone;

        RegisterEvent(new ContactUpdatedEvent(Key.Value, PersonId.Value, Type, previousPhone, phone.Formatted));
    }

    /// <summary>
    /// Gets the contact value as a string (for display purposes).
    /// </summary>
    public override string DisplayValue => Phone.Formatted;
}

