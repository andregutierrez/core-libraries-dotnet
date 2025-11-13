namespace Core.Libraries.Domain.Exceptions;

/// <summary>
/// Represents a base exception for unexpected errors during valid domain operations.
/// </summary>
/// <remarks>
/// Use this exception when a domain operation fails in a way that is not attributable
/// to business rules, validation, state transitions, or messaging errors.
/// </remarks>
public abstract class DomainOperationException : DomainException
{
    /// <summary>
    /// Gets the name of the operation that failed.
    /// </summary>
    public string OperationName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainOperationException"/> class.
    /// </summary>
    /// <param name="operationName">The name or label of the domain operation (e.g., "MergeAccounts").</param>
    /// <param name="domainContext">The domain context (aggregate/entity) where the error occurred.</param>
    /// <param name="message">A descriptive error message.</param>
    /// <param name="details">Optional structured data for diagnostics.</param>
    protected DomainOperationException(
        string operationName,
        string domainContext,
        string message,
        object? details = null)
        : base(
            message: message,
            errorCode: "DOMAIN_OPERATION_ERROR",
            domainContext: domainContext,
            details: details ?? new { Operation = operationName })
    {
        OperationName = operationName;
    }
}

