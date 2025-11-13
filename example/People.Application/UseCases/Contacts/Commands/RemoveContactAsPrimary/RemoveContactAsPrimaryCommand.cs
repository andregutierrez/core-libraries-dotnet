namespace People.Application.UseCases.Contacts.Commands.RemoveContactAsPrimary;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to remove the primary flag from a contact.
/// </summary>
public class RemoveContactAsPrimaryCommand : BaseCommand
{
    /// <summary>
    /// Gets the contact's alternate key.
    /// </summary>
    public Guid ContactKey { get; init; }
}

