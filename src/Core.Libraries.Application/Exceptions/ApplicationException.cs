namespace Core.Libraries.Application.Exceptions;

/// <summary>
/// Represents the base type for application-layer exceptions.
/// </summary>
/// <remarks>
/// Application exceptions are used to model violations, failures, or access issues that occur
/// during orchestration, validation, I/O interactions, or use-case-level logic outside the core domain.
/// </remarks>
[Serializable]
public abstract class ApplicationException : Exception
{
    /// <summary>
    /// A code that uniquely identifies the type of application error.
    /// This can be used for client error handling, logs, or telemetry.
    /// </summary>
    public virtual string? ErrorCode { get; protected set; }

    /// <summary>
    /// The application context (e.g., service, use case, or handler) in which the error occurred.
    /// </summary>
    public virtual string? ApplicationContext { get; protected set; }

    /// <summary>
    /// Optional structured metadata or object containing error details for diagnostics.
    /// </summary>
    public virtual object? Details { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationException"/> class.
    /// </summary>
    protected ApplicationException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    protected ApplicationException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationException"/> class with a message and additional error metadata.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="errorCode">A code identifying the specific application error.</param>
    /// <param name="applicationContext">The use case or service context where the exception occurred.</param>
    /// <param name="details">Optional diagnostic metadata.</param>
    protected ApplicationException(string message, string? errorCode, string? applicationContext, object? details = null)
        : base(message)
    {
        ErrorCode = errorCode;
        ApplicationContext = applicationContext;
        Details = details;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationException"/> class with a specified message and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception that caused the current exception.</param>
    protected ApplicationException(string message, Exception innerException)
        : base(message, innerException) { }
}
