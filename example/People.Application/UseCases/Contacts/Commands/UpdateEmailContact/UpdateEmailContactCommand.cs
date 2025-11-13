namespace People.Application.UseCases.Contacts.Commands;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to update an email contact's email address.
/// </summary>
public class UpdateEmailContactCommand : BaseCommand
{
    /// <summary>
    /// Gets the contact's alternate key.
    /// </summary>
    public Guid ContactKey { get; init; }

    /// <summary>
    /// Gets the new email address.
    /// </summary>
    public string Email { get; init; } = string.Empty;
}

