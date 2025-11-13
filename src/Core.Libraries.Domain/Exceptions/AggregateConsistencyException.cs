namespace Core.Libraries.Domain.Exceptions;

/// <summary>
/// Represents a base exception thrown when an aggregate root ends up in an invalid or inconsistent state.
/// </summary>
/// <remarks>
/// This exception should be used when an aggregate fails to maintain internal consistency,
/// such as broken invariants, conflicting state transitions, or misaligned child entity relationships.
/// </remarks>
public abstract class AggregateConsistencyException : DomainException
{
    /// <summary>
    /// Gets the identifier of the aggregate, if available.
    /// </summary>
    public string? AggregateId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateConsistencyException"/> class.
    /// </summary>
    /// <param name="message">The error message describing the inconsistency.</param>
    /// <param name="aggregateType">The name of the aggregate (e.g., "Order").</param>
    /// <param name="aggregateId">The identifier of the aggregate (optional).</param>
    /// <param name="details">Optional structured data for diagnostics.</param>
    protected AggregateConsistencyException(
        string message,
        string aggregateType,
        string? aggregateId = null,
        object? details = null)
        : base(
            message: message,
            errorCode: "AGGREGATE_CONSISTENCY_ERROR",
            domainContext: aggregateType,
            details: details ?? new { Aggregate = aggregateType, Id = aggregateId })
    {
        AggregateId = aggregateId;
    }
}
