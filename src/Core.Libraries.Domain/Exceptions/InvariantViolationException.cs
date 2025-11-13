namespace Core.Libraries.Domain.Exceptions;

/// <summary>
/// Represents a base exception for violations of entity or aggregate invariants within the domain model.
/// </summary>
/// <remarks>
/// Invariants are conditions that must always hold true for an entity to remain in a valid state.
/// Use this exception to explicitly enforce those rules when they are breached.
/// </remarks>
public abstract class InvariantViolationException : DomainException
{
    /// <summary>
    /// Gets the name or description of the invariant that was violated.
    /// </summary>
    public string Invariant { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvariantViolationException"/> class.
    /// </summary>
    /// <param name="invariant">The name or description of the violated invariant.</param>
    /// <param name="message">The exception message describing the failure.</param>
    /// <param name="domainContext">The context (entity/aggregate) where the invariant was violated.</param>
    /// <param name="details">Optional structured data for diagnostics.</param>
    protected InvariantViolationException(string invariant, string message, string domainContext, object? details = null)
        : base(message, "INVARIANT_VIOLATION", domainContext, details)
    {
        Invariant = invariant;
    }
}
