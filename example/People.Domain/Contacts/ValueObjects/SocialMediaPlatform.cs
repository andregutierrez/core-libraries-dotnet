namespace People.Domain.Contacts.ValueObjects;

using Core.Libraries.Domain.ValueObjects;

/// <summary>
/// Represents a social media platform.
/// </summary>
public class SocialMediaPlatform : Enumeration
{
    /// <summary>
    /// Facebook platform.
    /// </summary>
    public static readonly SocialMediaPlatform Facebook = new(1, "Facebook");

    /// <summary>
    /// Instagram platform.
    /// </summary>
    public static readonly SocialMediaPlatform Instagram = new(2, "Instagram");

    /// <summary>
    /// Twitter/X platform.
    /// </summary>
    public static readonly SocialMediaPlatform Twitter = new(3, "Twitter");

    /// <summary>
    /// LinkedIn platform.
    /// </summary>
    public static readonly SocialMediaPlatform LinkedIn = new(4, "LinkedIn");

    /// <summary>
    /// TikTok platform.
    /// </summary>
    public static readonly SocialMediaPlatform TikTok = new(5, "TikTok");

    /// <summary>
    /// YouTube platform.
    /// </summary>
    public static readonly SocialMediaPlatform YouTube = new(6, "YouTube");

    /// <summary>
    /// WhatsApp platform.
    /// </summary>
    public static readonly SocialMediaPlatform WhatsApp = new(7, "WhatsApp");

    /// <summary>
    /// Telegram platform.
    /// </summary>
    public static readonly SocialMediaPlatform Telegram = new(8, "Telegram");

    /// <summary>
    /// Discord platform.
    /// </summary>
    public static readonly SocialMediaPlatform Discord = new(9, "Discord");

    /// <summary>
    /// Initializes a new instance of the <see cref="SocialMediaPlatform"/> class.
    /// </summary>
    /// <param name="code">The unique code for the platform.</param>
    /// <param name="name">The name of the platform.</param>
    private SocialMediaPlatform(int code, string name) : base(code, name) { }

    /// <summary>
    /// Gets a platform by its code.
    /// </summary>
    /// <param name="code">The platform code.</param>
    /// <returns>The platform with the specified code, or <c>null</c> if not found.</returns>
    public static SocialMediaPlatform? FromCode(int code)
        => GetAll<SocialMediaPlatform>().FirstOrDefault(p => p.Code == code);

    /// <summary>
    /// Gets a platform by its name (case-insensitive).
    /// </summary>
    /// <param name="name">The platform name.</param>
    /// <returns>The platform with the specified name, or <c>null</c> if not found.</returns>
    public static SocialMediaPlatform? FromName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        return GetAll<SocialMediaPlatform>().FirstOrDefault(p =>
            p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}

