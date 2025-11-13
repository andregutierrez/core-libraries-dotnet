namespace People.Application.UseCases.Contacts.Commands.ImportContact;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to import a contact from an external system with a specific alternate key.
/// </summary>
public class ImportContactCommand : BaseCommand<ImportContactCommandResponse>
{
    /// <summary>
    /// Gets the alternate key from the external system.
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
    /// Gets the email address, if this is an email contact.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Gets the phone number, if this is a phone contact.
    /// </summary>
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// Gets the country code for the phone number, if this is a phone contact.
    /// </summary>
    public string? CountryCode { get; init; }

    /// <summary>
    /// Gets the social media platform ID, if this is a social media contact.
    /// </summary>
    public int? SocialMediaPlatformId { get; init; }

    /// <summary>
    /// Gets the username or account identifier, if this is a social media contact.
    /// </summary>
    public string? SocialMediaUsername { get; init; }

    /// <summary>
    /// Gets the profile URL, if this is a social media contact.
    /// </summary>
    public string? SocialMediaProfileUrl { get; init; }

    /// <summary>
    /// Gets a value indicating whether this is the primary contact of its type.
    /// </summary>
    public bool IsPrimary { get; init; }

    /// <summary>
    /// Gets additional notes or context for this contact.
    /// </summary>
    public string? Notes { get; init; }
}

/// <summary>
/// Response for the ImportContactCommand.
/// </summary>
public class ImportContactCommandResponse
{
    /// <summary>
    /// Gets the imported contact's alternate key.
    /// </summary>
    public Guid ContactKey { get; init; }
}

