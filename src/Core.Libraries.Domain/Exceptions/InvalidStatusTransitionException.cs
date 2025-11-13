namespace Core.Libraries.Domain.Exceptions;

/// <summary>
/// Represents an exception thrown when an invalid transition between entity statuses is attempted.
/// </summary>
/// <remarks>
/// This exception is typically used in domain models that implement state machines or workflows
/// where only certain transitions are allowed between status values.
/// </remarks>
public class InvalidStatusTransitionException : EntityStateException
{
    /// <summary>
    /// Gets the status from which the transition was attempted.
    /// </summary>
    public string FromStatus { get; }

    /// <summary>
    /// Gets the status that was attempted as the transition target.
    /// </summary>
    public string ToStatus { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidStatusTransitionException"/> class.
    /// </summary>
    /// <param name="domainContext">The entity or aggregate name (e.g., "Order").</param>
    /// <param name="fromStatus">The current status of the entity.</param>
    /// <param name="toStatus">The requested target status.</param>
    /// <param name="entityId">The identifier of the entity (optional).</param>
    /// <param name="details">Additional context or debugging data (optional).</param>
    public InvalidStatusTransitionException(
        string domainContext,
        string fromStatus,
        string toStatus,
        string? entityId = null,
        object? details = null)
        : base(
            message: $"Invalid status transition from '{fromStatus}' to '{toStatus}' in {domainContext}.",
            domainContext: domainContext,
            entityId: entityId,
            currentState: fromStatus,
            details: details ?? new { From = fromStatus, To = toStatus })
    {
        FromStatus = fromStatus;
        ToStatus = toStatus;
    }
}
