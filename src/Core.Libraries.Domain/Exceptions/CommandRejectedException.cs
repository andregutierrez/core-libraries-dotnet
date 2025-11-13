namespace Core.Libraries.Domain.Exceptions;

/// <summary>
/// Represents a base exception thrown when a domain command is rejected due to contextual or business constraints.
/// </summary>
/// <remarks>
/// This exception is typically used when a command is structurally valid but cannot be executed
/// due to unmet preconditions, conflicting entity state, or external constraints.
/// </remarks>
public abstract class CommandRejectedException : DomainException
{
    /// <summary>
    /// Gets the name or type of the rejected command.
    /// </summary>
    public string CommandName { get; }

    /// <summary>
    /// Gets the identifier of the target aggregate or entity, if applicable.
    /// </summary>
    public string? TargetId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandRejectedException"/> class.
    /// </summary>
    /// <param name="commandName">The name of the rejected command.</param>
    /// <param name="domainContext">The aggregate or domain context in which rejection occurred.</param>
    /// <param name="targetId">The target entity or aggregate identifier (optional).</param>
    /// <param name="message">A descriptive message for the rejection.</param>
    /// <param name="details">Optional diagnostic details.</param>
    protected CommandRejectedException(
        string commandName,
        string domainContext,
        string? targetId,
        string message,
        object? details = null)
        : base(
            message: message,
            errorCode: "COMMAND_REJECTED",
            domainContext: domainContext,
            details: details ?? new { Command = commandName, TargetId = targetId })
    {
        CommandName = commandName;
        TargetId = targetId;
    }
}
