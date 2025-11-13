using FluentValidation.Results;

namespace Core.Libraries.Domain.Notifications;

/// <summary>
/// Represents a context for managing validation notifications and errors.
/// </summary>
public interface INotificationContext
{
    /// <summary>
    /// Adds validation results (errors) to the notification context.
    /// </summary>
    /// <param name="validationResult">The validation result containing errors to add.</param>
    void AddNotifications(ValidationResult validationResult);

    /// <summary>
    /// Gets a value indicating whether there are any notifications (errors) in the context.
    /// </summary>
    bool HasNotifications { get; }

    /// <summary>
    /// Gets all notifications (errors) in the context.
    /// </summary>
    IReadOnlyList<string> Notifications { get; }
}

