using System.Runtime.Serialization;

namespace Core.Libraries.Domain.Exceptions;

/// <summary>
/// Represents the base exception type for domain-level errors.
/// </summary>
/// <remarks>
/// Domain exceptions are raised when business rules, invariants, or logical conditions
/// are violated in the core domain model. These exceptions are designed to be handled
/// explicitly by the application layer or policies.
/// </remarks>
[Serializable]
public abstract class DomainException : Exception
{
    /// <summary>
    /// Gets or sets the unique error code associated with the exception.
    /// Useful for classification, logging, or translation.
    /// </summary>
    public virtual string? ErrorCode { get; init; }

    /// <summary>
    /// Gets or sets the name of the domain context (entity, aggregate, etc.) where the exception occurred.
    /// </summary>
    public virtual string? DomainContext { get; init; }

    /// <summary>
    /// Gets or sets additional diagnostic details that may help in debugging or logging.
    /// </summary>
    public virtual object? Details { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainException"/> class.
    /// </summary>
    protected DomainException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that describes the exception.</param>
    protected DomainException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainException"/> class with a specified error message,
    /// error code, and domain context.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="errorCode">A domain-specific error code.</param>
    /// <param name="domainContext">The context (entity/aggregate) where the exception occurred.</param>
    /// <param name="details">Optional structured details for diagnostics.</param>
    protected DomainException(string message, string? errorCode, string? domainContext, object? details = null) : base(message)
    {
        ErrorCode = errorCode;
        DomainContext = domainContext;
        Details = details;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainException"/> class with a specified
    /// error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The exception that caused this exception.</param>
    protected DomainException(string message, Exception innerException)
        : base(message, innerException) { }
}