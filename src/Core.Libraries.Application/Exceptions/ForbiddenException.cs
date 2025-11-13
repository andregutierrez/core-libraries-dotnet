namespace Core.LibrariesApplication.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an authenticated user attempts to perform an unauthorized operation.
/// </summary>
/// <remarks>
/// This exception should be used when the user is authenticated, but lacks the necessary permissions
/// to access the requested resource or perform the operation. 
/// Maps to HTTP status code 403 (Forbidden).
/// </remarks>
public class ForbiddenException : ApplicationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenException"/> class with an optional message and additional details.
    /// </summary>
    /// <param name="reason">
    /// A message explaining why access is forbidden. If omitted, a generic message will be used.
    /// </param>
    /// <param name="details">
    /// Optional metadata or context relevant to the authorization failure.
    /// </param>
    public ForbiddenException(string? reason = null, object? details = null)
        : base(
            message: reason ?? "You do not have permission to perform this operation.",
            errorCode: "FORBIDDEN",
            applicationContext: "Authorization",
            details: details)
    {
    }
}
