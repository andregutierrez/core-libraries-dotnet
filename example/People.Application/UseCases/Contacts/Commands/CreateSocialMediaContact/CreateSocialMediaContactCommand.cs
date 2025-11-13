namespace People.Application.UseCases.Contacts.Commands;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to create a new social media contact for a person.
/// </summary>
public class CreateSocialMediaContactCommand : BaseCommand<CreateSocialMediaContactCommandResponse>
{
    /// <summary>
    /// Gets the person's alternate key this contact belongs to.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets the social media platform ID (e.g., 1 = Facebook, 2 = Instagram, etc.).
    /// </summary>
    public int SocialMediaPlatformId { get; init; }

    /// <summary>
    /// Gets the username or account identifier on the social media platform.
    /// </summary>
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// Gets the profile URL, if available.
    /// </summary>
    public string? ProfileUrl { get; init; }

    /// <summary>
    /// Gets a value indicating whether this is the primary social media contact.
    /// </summary>
    public bool IsPrimary { get; init; }

    /// <summary>
    /// Gets additional notes or context for this contact.
    /// </summary>
    public string? Notes { get; init; }
}

/// <summary>
/// Response for the CreateSocialMediaContactCommand.
/// </summary>
public class CreateSocialMediaContactCommandResponse
{
    /// <summary>
    /// Gets the created contact's alternate key.
    /// </summary>
    public Guid ContactKey { get; init; }
}

