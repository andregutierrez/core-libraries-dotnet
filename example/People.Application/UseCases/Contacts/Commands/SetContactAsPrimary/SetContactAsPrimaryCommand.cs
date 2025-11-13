namespace People.Application.UseCases.Contacts.Commands.SetContactAsPrimary;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to set a contact as the primary contact of its type.
/// </summary>
public class SetContactAsPrimaryCommand : BaseCommand
{
    /// <summary>
    /// Gets the contact's alternate key.
    /// </summary>
    public Guid ContactKey { get; init; }
}

