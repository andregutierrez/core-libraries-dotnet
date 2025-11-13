namespace People.Application.DTOs;

/// <summary>
/// Data transfer object representing detailed contact information.
/// Used for GET queries that return complete contact details.
/// </summary>
public class ContactDto
{
    /// <summary>
    /// Gets the contact's alternate key.
    /// </summary>
    public Guid ContactKey { get; init; }

    /// <summary>
    /// Gets the person reference this contact belongs to.
    /// </summary>
    public PersonReferenceDto Person { get; init; } = null!;

    /// <summary>
    /// Gets the type of contact.
    /// </summary>
    public int ContactTypeCode { get; init; }

    /// <summary>
    /// Gets the contact value (email address, phone number, or social media account).
    /// </summary>
    public string DisplayValue { get; init; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether this is the primary contact of its type.
    /// </summary>
    public bool IsPrimary { get; init; }

    /// <summary>
    /// Gets additional notes or context for this contact.
    /// </summary>
    public string? Notes { get; init; }

    /// <summary>
    /// Gets the email contact information, if this is an email contact.
    /// </summary>
    public EmailContactDto? Email { get; init; }

    /// <summary>
    /// Gets the phone contact information, if this is a phone contact (Phone, Mobile, or WhatsApp).
    /// </summary>
    public PhoneContactDto? Phone { get; init; }

    /// <summary>
    /// Gets the social media contact information, if this is a social media contact.
    /// </summary>
    public SocialMediaContactDto? SocialMedia { get; init; }
}

