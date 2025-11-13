namespace People.Domain.Persons.ValueObjects;

/// <summary>
/// Represents a person's full name with first name, last name, optional middle name, and optional social name.
/// </summary>
public record PersonName
{
    /// <summary>
    /// Gets the first name (given name) - registered name.
    /// </summary>
    public string FirstName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the last name (family name/surname) - registered name.
    /// </summary>
    public string LastName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the middle name, if any - registered name.
    /// </summary>
    public string? MiddleName { get; init; }

    /// <summary>
    /// Gets the social name (nome social), if any.
    /// This is the name the person prefers to be called, which may differ from their registered name.
    /// </summary>
    public string? SocialName { get; init; }

    /// <summary>
    /// Gets the full registered name formatted as "FirstName MiddleName LastName" or "FirstName LastName".
    /// </summary>
    public string FullName
    {
        get
        {
            var parts = new List<string> { FirstName };
            if (!string.IsNullOrWhiteSpace(MiddleName))
                parts.Add(MiddleName);
            parts.Add(LastName);
            return string.Join(" ", parts);
        }
    }

    /// <summary>
    /// Gets the display name to be used in the system.
    /// Returns the social name if available; otherwise, returns the full registered name.
    /// </summary>
    public string DisplayName => !string.IsNullOrWhiteSpace(SocialName) ? SocialName : FullName;

    /// <summary>
    /// Gets a value indicating whether the person has a social name.
    /// </summary>
    public bool HasSocialName => !string.IsNullOrWhiteSpace(SocialName);

    /// <summary>
    /// Protected constructor for EF Core and deserialization.
    /// </summary>
    protected PersonName() { }

    /// <summary>
    /// Initializes a new instance of <see cref="PersonName"/>.
    /// </summary>
    /// <param name="firstName">The first name. Cannot be null or whitespace.</param>
    /// <param name="lastName">The last name. Cannot be null or whitespace.</param>
    /// <param name="middleName">The optional middle name.</param>
    /// <param name="socialName">The optional social name (nome social).</param>
    /// <exception cref="ArgumentException">Thrown when firstName or lastName is null or whitespace.</exception>
    public PersonName(string firstName, string lastName, string? middleName = null, string? socialName = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or whitespace.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or whitespace.", nameof(lastName));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : middleName.Trim();
        SocialName = string.IsNullOrWhiteSpace(socialName) ? null : socialName.Trim();
    }

    /// <summary>
    /// Creates a PersonName from a full name string.
    /// Assumes format: "FirstName [MiddleName] LastName"
    /// </summary>
    /// <param name="fullName">The full name string.</param>
    /// <returns>A new PersonName instance.</returns>
    /// <exception cref="ArgumentException">Thrown when fullName is invalid.</exception>
    public static PersonName FromFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be null or whitespace.", nameof(fullName));

        var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2)
            throw new ArgumentException("Full name must contain at least first name and last name.", nameof(fullName));

        var firstName = parts[0];
        var lastName = parts[^1]; // Last element
        var middleName = parts.Length > 2
            ? string.Join(" ", parts[1..^1])
            : null;

        return new PersonName(firstName, lastName, middleName);
    }

    /// <summary>
    /// Returns the display name as a string.
    /// Uses the social name if available; otherwise, returns the full registered name.
    /// </summary>
    public override string ToString() => DisplayName;
}
