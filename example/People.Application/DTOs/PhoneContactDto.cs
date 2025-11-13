namespace People.Application.DTOs;

/// <summary>
/// Data transfer object representing phone contact information.
/// </summary>
public class PhoneContactDto
{
    /// <summary>
    /// Gets the phone number.
    /// </summary>
    public string PhoneNumber { get; init; } = string.Empty;

    /// <summary>
    /// Gets the country code.
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;
}

