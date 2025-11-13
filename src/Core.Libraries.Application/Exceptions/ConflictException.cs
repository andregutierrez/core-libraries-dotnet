namespace Core.Libraries.Application.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a request conflicts with the current state of the application or resource.
/// </summary>
/// <remarks>
/// Common scenarios include attempting to create a duplicate resource, update a stale entity,
/// or perform an operation that violates business invariants managed at the application level.
/// Maps to HTTP status code 409 (Conflict).
/// </remarks>
public class ConflictException : ApplicationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class with an optional message and details.
    /// </summary>
    /// <param name="message">
    /// A human-readable message describing the conflict. If not provided, a default message will be used.
    /// </param>
    /// <param name="details">
    /// Optional metadata or structured diagnostics to help explain the reason for the conflict.
    /// </param>
    public ConflictException(string? message = null, object? details = null)
        : base(
            message: message ?? "A conflict occurred with the current state of the resource.",
            errorCode: "CONFLICT",
            applicationContext: "Application",
            details: details)
    { }
}
