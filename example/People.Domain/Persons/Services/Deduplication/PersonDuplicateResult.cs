namespace People.Domain.Persons.Services;

using People.Domain.Persons.Entities;

/// <summary>
/// Represents a result of duplicate person detection with similarity information.
/// </summary>
public record PersonDuplicateResult
{
    /// <summary>
    /// Gets the duplicate person found.
    /// </summary>
    public Person Person { get; init; } = null!;

    /// <summary>
    /// Gets the similarity score between the searched person and this duplicate (0.0 to 1.0).
    /// A score of 1.0 indicates an exact match.
    /// </summary>
    public double SimilarityScore { get; init; }

    /// <summary>
    /// Gets the reason why this person is considered a duplicate.
    /// </summary>
    public DuplicateMatchReason MatchReason { get; init; }

    /// <summary>
    /// Initializes a new instance of <see cref="PersonDuplicateResult"/>.
    /// </summary>
    /// <param name="person">The duplicate person found.</param>
    /// <param name="similarityScore">The similarity score (0.0 to 1.0).</param>
    /// <param name="matchReason">The reason for the match.</param>
    public PersonDuplicateResult(Person person, double similarityScore, DuplicateMatchReason matchReason)
    {
        ArgumentNullException.ThrowIfNull(person);
        if (similarityScore < 0.0 || similarityScore > 1.0)
            throw new ArgumentOutOfRangeException(nameof(similarityScore), "Similarity score must be between 0.0 and 1.0.");

        Person = person;
        SimilarityScore = similarityScore;
        MatchReason = matchReason;
    }
}

/// <summary>
/// Represents the reason why a person is considered a duplicate.
/// </summary>
public enum DuplicateMatchReason
{
    /// <summary>
    /// Exact match on name and birth date.
    /// </summary>
    ExactMatch = 1,

    /// <summary>
    /// Similar name with exact birth date match.
    /// </summary>
    SimilarNameExactBirthDate = 2,

    /// <summary>
    /// Exact name match with similar birth date (within a few days).
    /// </summary>
    ExactNameSimilarBirthDate = 3,

    /// <summary>
    /// Similar name and similar birth date.
    /// </summary>
    SimilarNameSimilarBirthDate = 4,

    /// <summary>
    /// Match by external identifier.
    /// </summary>
    ExternalIdentifier = 5
}

