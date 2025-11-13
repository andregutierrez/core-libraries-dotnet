namespace Core.Libraries.Domain.Exceptions;

/// <summary>
/// Represents an exception thrown when an operation is invalid due to the current state of an entity or aggregate.
/// </summary>
/// <remarks>
/// This exception is typically used to enforce state transitions or guard conditions within aggregate methods.
/// </remarks>
public class EntityStateException : DomainException
{
    /// <summary>
    /// Gets the identifier of the entity that caused the exception, if applicable.
    /// </summary>
    public string? EntityId { get; }

    /// <summary>
    /// Gets the current state of the entity at the time the exception was thrown.
    /// </summary>
    public string? CurrentState { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityStateException"/> class.
    /// </summary>
    /// <param name="message">A message that describes the error.</param>
    /// <param name="domainContext">The entity or aggregate where the state violation occurred.</param>
    /// <param name="entityId">The identifier of the entity (optional).</param>
    /// <param name="currentState">The current state value or label (optional).</param>
    /// <param name="details">Optional extra context for diagnostics.</param>
    public EntityStateException(
        string message,
        string domainContext,
        string? entityId = null,
        string? currentState = null,
        object? details = null)
        : base(message, "INVALID_ENTITY_STATE", domainContext, details)
    {
        EntityId = entityId;
        CurrentState = currentState;
    }
}
