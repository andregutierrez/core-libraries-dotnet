namespace People.Application.DTOs;

/// <summary>
/// Data transfer object representing social media contact information.
/// </summary>
public class SocialMediaContactDto
{
    /// <summary>
    /// Gets the social media platform code.
    /// </summary>
    public int PlatformCode { get; init; }

    /// <summary>
    /// Gets the social media username.
    /// </summary>
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// Gets the social media profile URL.
    /// </summary>
    public string? ProfileUrl { get; init; }
}

