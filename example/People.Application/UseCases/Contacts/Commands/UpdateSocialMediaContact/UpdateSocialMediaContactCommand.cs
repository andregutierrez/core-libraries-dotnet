namespace People.Application.UseCases.Contacts.Commands;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to update a social media contact's account information.
/// </summary>
public class UpdateSocialMediaContactCommand : BaseCommand
{
    /// <summary>
    /// Gets the contact's alternate key.
    /// </summary>
    public Guid ContactKey { get; init; }

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
}

