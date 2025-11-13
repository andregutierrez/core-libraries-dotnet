namespace People.Domain.Contacts.ValueObjects;

using System.Text.RegularExpressions;

/// <summary>
/// Represents a phone number with validation.
/// </summary>
public record PhoneNumber
{
    private static readonly Regex _digitsOnly = new(@"\D", RegexOptions.Compiled);

    /// <summary>
    /// Gets the phone number value (digits only).
    /// </summary>
    public string Value { get; init; } = string.Empty;

    /// <summary>
    /// Gets the formatted phone number (with country code, area code, and number).
    /// </summary>
    public string Formatted { get; init; } = string.Empty;

    /// <summary>
    /// Protected constructor for EF Core and deserialization.
    /// </summary>
    protected PhoneNumber() { }

    /// <summary>
    /// Initializes a new instance of <see cref="PhoneNumber"/>.
    /// </summary>
    /// <param name="phoneNumber">The phone number string (with or without formatting).</param>
    /// <exception cref="ArgumentException">Thrown when phoneNumber is invalid.</exception>
    public PhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be null or whitespace.", nameof(phoneNumber));

        var digits = _digitsOnly.Replace(phoneNumber, "");

        // Basic validation: must have at least 10 digits (typical minimum for phone numbers)
        if (digits.Length < 10 || digits.Length > 15)
            throw new ArgumentException("Phone number must have between 10 and 15 digits.", nameof(phoneNumber));

        Value = digits;
        Formatted = FormatPhoneNumber(digits);
    }

    private static string FormatPhoneNumber(string digits)
    {
        // Simple formatting: assumes country code + area code + number
        if (digits.Length == 10)
        {
            // Format: (XX) XXXXX-XXXX
            return $"({digits[0..2]}) {digits[2..7]}-{digits[7..]}";
        }
        else if (digits.Length == 11)
        {
            // Format: (XX) 9XXXX-XXXX (Brazilian mobile)
            return $"({digits[0..2]}) {digits[2..7]}-{digits[7..]}";
        }
        else if (digits.Length > 11)
        {
            // Format: +XX (XX) XXXXX-XXXX (with country code)
            var countryCode = digits[0..(digits.Length - 10)];
            var areaCode = digits[(digits.Length - 10)..(digits.Length - 8)];
            var number = digits[(digits.Length - 8)..];
            return $"+{countryCode} ({areaCode}) {number[0..5]}-{number[5..]}";
        }

        return digits;
    }

    /// <summary>
    /// Returns the formatted phone number as a string.
    /// </summary>
    public override string ToString() => Formatted;
}

