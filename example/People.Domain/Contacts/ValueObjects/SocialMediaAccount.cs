namespace People.Domain.Contacts.ValueObjects;

/// <summary>
/// Represents a social media account with platform and username/handle.
/// </summary>
public record SocialMediaAccount
{
    /// <summary>
    /// Gets the social media platform.
    /// </summary>
    public SocialMediaPlatform Platform { get; init; } = null!;

    /// <summary>
    /// Gets the username or handle on the platform.
    /// </summary>
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// Gets the full URL to the profile, if available.
    /// </summary>
    public string? ProfileUrl { get; init; }

    /// <summary>
    /// Protected constructor for EF Core and deserialization.
    /// </summary>
    protected SocialMediaAccount() { }

    /// <summary>
    /// Initializes a new instance of <see cref="SocialMediaAccount"/>.
    /// </summary>
    /// <param name="platform">The social media platform. Cannot be null.</param>
    /// <param name="username">The username or handle. Cannot be null or whitespace.</param>
    /// <param name="profileUrl">Optional profile URL.</param>
    /// <exception cref="ArgumentNullException">Thrown when platform is null.</exception>
    /// <exception cref="ArgumentException">Thrown when username is null or whitespace.</exception>
    public SocialMediaAccount(SocialMediaPlatform platform, string username, string? profileUrl = null)
    {
        ArgumentNullException.ThrowIfNull(platform);

        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be null or whitespace.", nameof(username));

        Platform = platform;
        Username = username.Trim();
        ProfileUrl = string.IsNullOrWhiteSpace(profileUrl) ? null : profileUrl.Trim();
    }

    /// <summary>
    /// Gets the display name for this social media account.
    /// Format: "Platform: @username" or "Platform: username".
    /// </summary>
    public string DisplayName
    {
        get
        {
            var prefix = Username.StartsWith('@') ? string.Empty : "@";
            return $"{Platform.Name}: {prefix}{Username}";
        }
    }

    /// <summary>
    /// Returns the display name as a string.
    /// </summary>
    public override string ToString() => DisplayName;
}

