namespace People.Domain.Persons.Services.Deduplication;

using People.Domain.Persons.Entities;

/// <summary>
/// Represents the result of validating person creation for duplicates.
/// </summary>
public record PersonDeduplicationValidationResult
{
    /// <summary>
    /// Gets a value indicating whether the person can be created (no duplicates found).
    /// </summary>
    public bool CanCreate { get; init; }

    /// <summary>
    /// Gets the collection of potential duplicates found.
    /// </summary>
    public IReadOnlyList<PersonDuplicateResult> PotentialDuplicates { get; init; } = Array.Empty<PersonDuplicateResult>();

    /// <summary>
    /// Gets the validation message explaining the result.
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of <see cref="PersonDeduplicationValidationResult"/>.
    /// </summary>
    /// <param name="canCreate">Whether the person can be created.</param>
    /// <param name="potentialDuplicates">The collection of potential duplicates.</param>
    /// <param name="message">The validation message.</param>
    public PersonDeduplicationValidationResult(
        bool canCreate,
        IReadOnlyList<PersonDuplicateResult>? potentialDuplicates = null,
        string? message = null)
    {
        CanCreate = canCreate;
        PotentialDuplicates = potentialDuplicates ?? Array.Empty<PersonDuplicateResult>();
        Message = message ?? (canCreate ? "No duplicates found. Person can be created." : "Potential duplicates found.");
    }

    /// <summary>
    /// Creates a validation result indicating that the person can be created (no duplicates).
    /// </summary>
    /// <returns>A validation result indicating success.</returns>
    public static PersonDeduplicationValidationResult Success()
        => new(true, null, "No duplicates found. Person can be created.");

    /// <summary>
    /// Creates a validation result indicating that duplicates were found.
    /// </summary>
    /// <param name="potentialDuplicates">The collection of potential duplicates.</param>
    /// <param name="message">Optional custom message.</param>
    /// <returns>A validation result indicating duplicates were found.</returns>
    public static PersonDeduplicationValidationResult DuplicatesFound(
        IReadOnlyList<PersonDuplicateResult> potentialDuplicates,
        string? message = null)
    {
        ArgumentNullException.ThrowIfNull(potentialDuplicates);
        return new(false, potentialDuplicates, message ?? $"Found {potentialDuplicates.Count} potential duplicate(s).");
    }
}

