namespace Core.Libraries.Domain.Exceptions;

/// <summary>
/// Represents a base exception thrown when a domain message fails to deserialize due to invalid structure or content.
/// </summary>
/// <remarks>
/// This exception is used to indicate that a message (event, command, or payload) could not be interpreted
/// as expected by the domain. This may occur due to missing fields, schema mismatches, or invalid formatting.
/// </remarks>
public abstract class MessageDeserializationException : DomainException
{
    /// <summary>
    /// Gets the raw message content that failed to deserialize, if available.
    /// </summary>
    public string? RawContent { get; }

    /// <summary>
    /// Gets the expected message type the system attempted to deserialize into.
    /// </summary>
    public string? TargetType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageDeserializationException"/> class.
    /// </summary>
    /// <param name="message">The high-level error message.</param>
    /// <param name="targetType">The name of the expected target type.</param>
    /// <param name="rawContent">The raw message content (optional).</param>
    /// <param name="domainContext">The domain context or message origin (e.g., "EventBus", "ApiGateway").</param>
    /// <param name="details">Optional diagnostic data.</param>
    protected MessageDeserializationException(
        string message,
        string targetType,
        string? rawContent,
        string domainContext,
        object? details = null)
        : base(
            message: message,
            errorCode: "MESSAGE_DESERIALIZATION_FAILED",
            domainContext: domainContext,
            details: details ?? new { Target = targetType, Raw = rawContent })
    {
        TargetType = targetType;
        RawContent = rawContent;
    }
}
