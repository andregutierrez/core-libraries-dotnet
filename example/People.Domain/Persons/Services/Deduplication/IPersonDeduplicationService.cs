namespace People.Domain.Persons.Services;

using People.Domain.Persons.Entities;
using People.Domain.Persons.ValueObjects;

/// <summary>
/// Service for detecting and managing duplicate persons in the system.
/// </summary>
public interface IPersonDeduplicationService
{
    /// <summary>
    /// Checks if a person with the given name and birth date already exists.
    /// </summary>
    /// <param name="name">The person's name.</param>
    /// <param name="birthDate">The person's birth date. Optional.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the existing person if found; otherwise, <c>null</c>.
    /// </returns>
    Task<Person?> CheckDuplicateAsync(PersonName name, BirthDate? birthDate = null);

    /// <summary>
    /// Searches for potential duplicate persons based on name and birth date similarity.
    /// </summary>
    /// <param name="name">The person's name to search for.</param>
    /// <param name="birthDate">The person's birth date. Optional.</param>
    /// <param name="similarityThreshold">The minimum similarity threshold (0.0 to 1.0). Defaults to 0.8.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a collection of potential duplicate persons with their similarity scores.
    /// </returns>
    Task<IEnumerable<PersonDuplicateResult>> FindPotentialDuplicatesAsync(
        PersonName name,
        BirthDate? birthDate = null,
        double similarityThreshold = 0.8);

    /// <summary>
    /// Checks if a person with the given external identifier already exists.
    /// </summary>
    /// <param name="identifierType">The type of external identifier.</param>
    /// <param name="externalId">The external identifier value.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the existing person if found; otherwise, <c>null</c>.
    /// </returns>
    Task<Person?> CheckDuplicateByIdentifierAsync(
        Core.Libraries.Domain.Entities.Identifiers.IdentifierType identifierType,
        string externalId);

    /// <summary>
    /// Validates if a person can be created without creating duplicates.
    /// </summary>
    /// <param name="name">The person's name.</param>
    /// <param name="birthDate">The person's birth date. Optional.</param>
    /// <param name="allowSimilar">Whether to allow similar (but not exact) duplicates. Defaults to <c>false</c>.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the validation result with information about potential duplicates.
    /// </returns>
    Task<PersonDeduplicationValidationResult> ValidateCreationAsync(
        PersonName name,
        BirthDate? birthDate = null,
        bool allowSimilar = false);
}

