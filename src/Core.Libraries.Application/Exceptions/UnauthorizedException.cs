namespace Core.Libraries.Application.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a request is made without valid authentication.
/// </summary>
/// <remarks>
/// This exception should be used when no valid identity is present in the context of the request.
/// It maps to HTTP status code 401 (Unauthorized).
/// </remarks>
public class UnauthorizedException : ApplicationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedException"/> class.
    /// </summary>
    /// <param name="details">
    /// Optional metadata or diagnostic information about the authentication failure.
    /// </param>
    public UnauthorizedException(object? details = null)
        : base(
            message: "Authentication is required to access this resource.",
            errorCode: "UNAUTHORIZED",
            applicationContext: "Security",
            details: details)
    {
    }
}
