namespace Core.Libraries.Domain.Exceptions;

/// <summary>
/// Represents a base exception for domain validation failures such as invalid input, required fields,
/// or format violations that prevent an operation from proceeding.
/// </summary>
/// <remarks>
/// Unlike <see cref="BusinessRuleViolationException"/>, this exception typically applies to user-provided
/// or external input that fails to meet defined structural or semantic constraints.
/// </remarks>
public abstract class ValidationException : DomainException
{
    /// <summary>
    /// Gets a collection of validation errors associated with the exception.
    /// </summary>
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class.
    /// </summary>
    /// <param name="message">A general error message for the validation failure.</param>
    /// <param name="domainContext">The entity or aggregate context where the validation occurred.</param>
    /// <param name="errors">A dictionary containing field names and associated error messages.</param>
    protected ValidationException(string message, string domainContext, IDictionary<string, string[]> errors)
        : base(message, "VALIDATION_FAILURE", domainContext, details: errors)
    {
        Errors = new Dictionary<string, string[]>(errors);
    }
}
