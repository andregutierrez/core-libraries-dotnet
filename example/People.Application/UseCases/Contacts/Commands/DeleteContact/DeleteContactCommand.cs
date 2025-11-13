namespace People.Application.UseCases.Contacts.Commands.DeleteContact;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to delete a contact.
/// </summary>
public class DeleteContactCommand : BaseCommand
{
    /// <summary>
    /// Gets the contact's alternate key.
    /// </summary>
    public Guid ContactKey { get; init; }
}

