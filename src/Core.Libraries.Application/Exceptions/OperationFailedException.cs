namespace Core.Libraries.Application.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an application-level operation fails
/// due to technical, orchestration, or infrastructure-related issues not tied to domain rules.
/// </summary>
/// <remarks>
/// This exception is suitable for scenarios such as workflow failures, service coordination errors,
/// or unexpected conditions that prevent the completion of a use case.
/// </remarks>
public class OperationFailedException : ApplicationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OperationFailedException"/> class.
    /// </summary>
    /// <param name="operationName">The logical name of the failed operation or use case.</param>
    /// <param name="message">
    /// A descriptive error message. If not specified, a default message based on the operation name will be used.
    /// </param>
    /// <param name="details">
    /// Optional structured metadata or diagnostics related to the failure.
    /// </param>
    public OperationFailedException(string operationName, string? message = null, object? details = null)
        : base(
            message: message ?? $"The operation '{operationName}' could not be completed.",
            errorCode: "OPERATION_FAILED",
            applicationContext: operationName,
            details: details)
    {
    }
}
