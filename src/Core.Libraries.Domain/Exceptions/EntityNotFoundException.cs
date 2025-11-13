namespace Core.Libraries.Domain.Exceptions;

/// <summary>
/// Represents an exception thrown when a specific entity instance could not be found in the domain context.
/// </summary>
/// <remarks>
/// This exception is typically used when an entity is referenced but not present in memory,
/// event streams, or expected repositories, and its absence breaks domain invariants or workflows.
/// </remarks>
public class EntityNotFoundException : DomainException
{
    /// <summary>
    /// Gets the type or name of the entity that was not found.
    /// </summary>
    public string EntityName { get; }

    /// <summary>
    /// Gets the identifier of the missing entity, if available.
    /// </summary>
    public object? EntityId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
    /// </summary>
    /// <param name="entityName">The name or type of the missing entity (e.g., "Customer").</param>
    /// <param name="entityId">The identifier of the missing entity (optional).</param>
    /// <param name="details">Optional structured information for diagnostics.</param>
    public EntityNotFoundException(string entityName, object? entityId = null, object? details = null)
        : base(
            message: $"The specified {entityName} could not be found.",
            errorCode: "ENTITY_NOT_FOUND",
            domainContext: entityName,
            details: details ?? new { Entity = entityName, Id = entityId })
    {
        EntityName = entityName;
        EntityId = entityId;
    }
}
