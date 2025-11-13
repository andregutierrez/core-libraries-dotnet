namespace People.Application.UseCases.Contacts.Commands.UpdateContactNotes;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to update the notes for a contact.
/// </summary>
public record UpdateContactNotesCommand : BaseCommand
{
    /// <summary>
    /// Gets the contact's alternate key.
    /// </summary>
    public Guid ContactKey { get; init; }

    /// <summary>
    /// Gets the new notes. Can be null to remove notes.
    /// </summary>
    public string? Notes { get; init; }
}

