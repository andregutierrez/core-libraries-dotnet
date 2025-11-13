namespace People.Domain.Contacts.Entities;

using Core.Libraries.Domain.Entities;
using People.Domain.Contacts.Events;
using People.Domain.Contacts.ValueObjects;

/// <summary>
/// Represents a social media contact for a person.
/// </summary>
public class SocialMediaContact : Contact
{
    /// <summary>
    /// Gets the social media account.
    /// </summary>
    public SocialMediaAccount SocialMedia { get; protected set; } = null!;

    /// <summary>
    /// Protected constructor for EF Core.
    /// </summary>
    protected SocialMediaContact() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="SocialMediaContact"/>.
    /// </summary>
    /// <param name="key">The alternate key for this contact.</param>
    /// <param name="personId">The person's entity ID this contact belongs to.</param>
    /// <param name="socialMedia">The social media account.</param>
    /// <param name="isPrimary">Whether this is the primary social media contact.</param>
    /// <param name="notes">Optional notes for this contact.</param>
    private SocialMediaContact(AlternateKey key, EntityId personId, SocialMediaAccount socialMedia, bool isPrimary = false, string? notes = null)
        : base(key, personId, ContactType.SocialMedia, isPrimary, notes)
    {
        ArgumentNullException.ThrowIfNull(socialMedia);
        SocialMedia = socialMedia;
    }

    /// <summary>
    /// Creates a new social media contact.
    /// </summary>
    /// <param name="personId">The person's entity ID this contact belongs to.</param>
    /// <param name="socialMedia">The social media account.</param>
    /// <param name="isPrimary">Whether this is the primary social media contact.</param>
    /// <param name="notes">Optional notes for this contact.</param>
    /// <returns>A new instance of <see cref="SocialMediaContact"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when socialMedia is null.</exception>
    public static SocialMediaContact Create(EntityId personId, SocialMediaAccount socialMedia, bool isPrimary = false, string? notes = null)
    {
        ArgumentNullException.ThrowIfNull(personId);
        ArgumentNullException.ThrowIfNull(socialMedia);

        var contact = new SocialMediaContact(AlternateKey.New(), personId, socialMedia, isPrimary, notes);
        contact.RegisterEvent(new ContactCreatedEvent(contact.Key.Value, personId.Value, ContactType.SocialMedia, socialMedia.DisplayName));
        return contact;
    }

    /// <summary>
    /// Imports a social media contact from an external system with a specific alternate key.
    /// </summary>
    /// <param name="key">The alternate key from the external system. Cannot be null.</param>
    /// <param name="personId">The person's entity ID this contact belongs to.</param>
    /// <param name="socialMedia">The social media account.</param>
    /// <param name="isPrimary">Whether this is the primary social media contact.</param>
    /// <param name="notes">Optional notes for this contact.</param>
    /// <returns>A new instance of <see cref="SocialMediaContact"/> with the specified alternate key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key or socialMedia is null.</exception>
    public static SocialMediaContact Import(AlternateKey key, EntityId personId, SocialMediaAccount socialMedia, bool isPrimary = false, string? notes = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(personId);
        ArgumentNullException.ThrowIfNull(socialMedia);

        var contact = new SocialMediaContact(key, personId, socialMedia, isPrimary, notes);
        contact.RegisterEvent(new ContactCreatedEvent(contact.Key.Value, personId.Value, ContactType.SocialMedia, socialMedia.DisplayName));
        return contact;
    }

    /// <summary>
    /// Updates the social media account for this contact.
    /// </summary>
    /// <param name="socialMedia">The new social media account.</param>
    /// <exception cref="ArgumentNullException">Thrown when socialMedia is null.</exception>
    public void UpdateSocialMedia(SocialMediaAccount socialMedia)
    {
        ArgumentNullException.ThrowIfNull(socialMedia);

        var previousAccount = SocialMedia.DisplayName;
        SocialMedia = socialMedia;

        RegisterEvent(new ContactUpdatedEvent(Key.Value, PersonId.Value, Type, previousAccount, socialMedia.DisplayName));
    }

    /// <summary>
    /// Gets the contact value as a string (for display purposes).
    /// </summary>
    public override string DisplayValue => SocialMedia.DisplayName;
}

