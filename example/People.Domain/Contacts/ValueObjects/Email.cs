namespace People.Domain.Contacts.ValueObjects;

using System.Net.Mail;

/// <summary>
/// Represents an email address with validation and normalization.
/// </summary>
public record Email
{
    /// <summary>
    /// Gets the normalized email address (lowercase).
    /// </summary>
    public string Value { get; init; } = string.Empty;

    /// <summary>
    /// Gets the local part of the email (before the '@').
    /// </summary>
    public string LocalPart
    {
        get
        {
            var index = Value.IndexOf('@');
            return index > 0 ? Value[..index] : string.Empty;
        }
    }

    /// <summary>
    /// Gets the domain part of the email (after the '@').
    /// </summary>
    public string Domain
    {
        get
        {
            var index = Value.IndexOf('@');
            return index >= 0 && index < Value.Length - 1 ? Value[(index + 1)..] : string.Empty;
        }
    }

    /// <summary>
    /// Protected constructor for EF Core and deserialization.
    /// </summary>
    protected Email() { }

    /// <summary>
    /// Initializes a new instance of <see cref="Email"/>.
    /// </summary>
    /// <param name="email">The email address string.</param>
    /// <exception cref="ArgumentException">Thrown when email is null, whitespace, or invalid format.</exception>
    public Email(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or whitespace.", nameof(email));

        try
        {
            var mailAddress = new MailAddress(email);
            Value = mailAddress.Address.ToLowerInvariant();
        }
        catch (FormatException)
        {
            throw new ArgumentException("Invalid email format.", nameof(email));
        }
    }

    /// <summary>
    /// Returns the email address as a string.
    /// </summary>
    public override string ToString() => Value;
}

