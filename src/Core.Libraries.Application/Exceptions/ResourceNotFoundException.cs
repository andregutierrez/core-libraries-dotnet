namespace Core.LibrariesApplication.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a requested resource could not be found.
/// </summary>
/// <remarks>
/// This exception is commonly used in application services or use cases when an entity
/// is not found by a given identifier or filter. It maps to HTTP status code 404 (Not Found).
/// </remarks>
public class ResourceNotFoundException : ApplicationException
{
    /// <summary>
    /// Gets the logical name or type of the resource that was not found (e.g., "User", "Order").
    /// </summary>
    public string ResourceName { get; }

    /// <summary>
    /// Gets the key or identifier that was used to attempt to retrieve the resource.
    /// </summary>
    public object? ResourceKey { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceNotFoundException"/> class.
    /// </summary>
    /// <param name="resourceName">The name or type of the missing resource.</param>
    /// <param name="resourceKey">The identifier or lookup key used in the search (optional).</param>
    /// <param name="details">
    /// Optional metadata or additional context about the lookup failure. If not provided, a default structure is used.
    /// </param>
    public ResourceNotFoundException(string resourceName, object? resourceKey = null, object? details = null)
        : base(
            message: $"The requested resource '{resourceName}' could not be found.",
            errorCode: "RESOURCE_NOT_FOUND",
            applicationContext: resourceName,
            details: details ?? new { Resource = resourceName, Key = resourceKey })
    {
        ResourceName = resourceName;
        ResourceKey = resourceKey;
    }
}
