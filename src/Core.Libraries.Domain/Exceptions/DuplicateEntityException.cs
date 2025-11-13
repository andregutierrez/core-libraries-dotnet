namespace Core.Libraries.Domain.Exceptions;

/// <summary>
/// Represents an exception thrown when an attempt is made to create or register an entity
/// that already exists with the same identity or business key.
/// </summary>
/// <remarks>
/// This exception is typically used to enforce business-level uniqueness constraints,
/// such as email, document numbers, or composite business keys.
/// </remarks>
public class DuplicateEntityException : DomainException
{
    /// <summary>
    /// Gets the name or type of the entity that caused the duplication.
    /// </summary>
    public string EntityName { get; }

    /// <summary>
    /// Gets the conflicting key or identifier value, if available.
    /// </summary>
    public object? ConflictKey { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateEntityException"/> class.
    /// </summary>
    /// <param name="entityName">The name of the entity involved (e.g., "User").</param>
    /// <param name="conflictKey">The key or identifier that caused the conflict (optional).</param>
    /// <param name="details">Optional diagnostic information.</param>
    public DuplicateEntityException(string entityName, object? conflictKey = null, object? details = null)
        : base(
            message: $"A {entityName} with the same key already exists.",
            errorCode: "DUPLICATE_ENTITY",
            domainContext: entityName,
            details: details ?? new { Entity = entityName, Key = conflictKey })
    {
        EntityName = entityName;
        ConflictKey = conflictKey;
    }
}
