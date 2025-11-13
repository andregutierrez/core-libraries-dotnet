namespace Core.Libraries.Domain.Exceptions;

/// <summary>
/// Represents an exception thrown when an entity or value object has an invalid or malformed identifier.
/// </summary>
/// <remarks>
/// This exception is typically used to enforce identifier integrity rules in domain models,
/// such as required IDs, format constraints, or mismatched identifier types.
/// </remarks>
public class InvalidIdentifierException : DomainException
{
    /// <summary>
    /// Gets the name of the identifier field or type that is invalid.
    /// </summary>
    public string IdentifierName { get; }

    /// <summary>
    /// Gets the value of the invalid identifier, if provided.
    /// </summary>
    public object? ProvidedValue { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidIdentifierException"/> class.
    /// </summary>
    /// <param name="identifierName">The name or label of the identifier (e.g., "UserId").</param>
    /// <param name="providedValue">The value that caused the failure (optional).</param>
    /// <param name="domainContext">The entity or aggregate name related to the error.</param>
    /// <param name="details">Optional details for diagnostics.</param>
    public InvalidIdentifierException(
        string identifierName,
        object? providedValue,
        string domainContext,
        object? details = null)
        : base(
            message: $"Invalid identifier '{identifierName}' for domain context '{domainContext}'.",
            errorCode: "INVALID_IDENTIFIER",
            domainContext: domainContext,
            details: details ?? new { Identifier = identifierName, Value = providedValue })
    {
        IdentifierName = identifierName;
        ProvidedValue = providedValue;
    }
}
