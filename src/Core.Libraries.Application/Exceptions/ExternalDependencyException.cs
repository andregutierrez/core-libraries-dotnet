namespace Core.LibrariesApplication.Exceptions;

/// <summary>
/// Represents an exception that occurs when an external service or system dependency fails.
/// </summary>
/// <remarks>
/// This exception is commonly used to wrap failures from third-party APIs, databases, queues,
/// or other infrastructure services that the application relies upon. 
/// Maps to HTTP status code 503 or 502, depending on the nature of the failure.
/// </remarks>
public class ExternalDependencyException : ApplicationException
{
    /// <summary>
    /// Gets the name or identifier of the external dependency where the failure occurred.
    /// </summary>
    public string DependencyName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalDependencyException"/> class.
    /// </summary>
    /// <param name="dependencyName">The name of the external service or dependency that failed.</param>
    /// <param name="message">
    /// An optional error message describing the failure. If not specified, a default message will be generated.
    /// </param>
    /// <param name="details">
    /// Optional diagnostic information or structured data related to the external failure.
    /// </param>
    public ExternalDependencyException(string dependencyName, string? message = null, object? details = null)
        : base(
            message: message ?? $"A failure occurred while communicating with external service '{dependencyName}'.",
            errorCode: "EXTERNAL_DEPENDENCY_FAILURE",
            applicationContext: dependencyName,
            details: details)
    {
        DependencyName = dependencyName;
    }
}
