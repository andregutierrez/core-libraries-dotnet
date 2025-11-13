namespace People.Application.UseCases.Contacts.Queries.GetContactsByPersonKey;

using Core.Libraries.Application.Queries;

/// <summary>
/// Query to get all contacts for a person.
/// </summary>
public class GetContactsByPersonKeyQuery : IQuery<GetContactsByPersonKeyQueryResponse>
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }
}

/// <summary>
/// Response for the GetContactsByPersonKeyQuery.
/// </summary>
public class GetContactsByPersonKeyQueryResponse
{
    /// <summary>
    /// Gets the collection of contacts.
    /// </summary>
    public IReadOnlyList<ContactDto> Contacts { get; init; } = Array.Empty<ContactDto>();
}

/// <summary>
/// Data transfer object for contact information.
/// </summary>
public class ContactDto
{
    /// <summary>
    /// Gets the contact's alternate key.
    /// </summary>
    public Guid ContactKey { get; init; }

    /// <summary>
    /// Gets the person's alternate key this contact belongs to.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets the type of contact (Email = 1, Phone = 2, Mobile = 3, WhatsApp = 4, SocialMedia = 5).
    /// </summary>
    public int ContactTypeId { get; init; }

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
    /// Gets the email address, if this is an email contact.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Gets the phone number, if this is a phone contact.
    /// </summary>
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// Gets the country code, if this is a phone contact.
    /// </summary>
    public string? CountryCode { get; init; }

    /// <summary>
    /// Gets the social media platform ID, if this is a social media contact.
    /// </summary>
    public int? SocialMediaPlatformId { get; init; }

    /// <summary>
    /// Gets the social media username, if this is a social media contact.
    /// </summary>
    public string? SocialMediaUsername { get; init; }

    /// <summary>
    /// Gets the social media profile URL, if this is a social media contact.
    /// </summary>
    public string? SocialMediaProfileUrl { get; init; }
}

