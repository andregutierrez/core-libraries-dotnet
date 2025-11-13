namespace People.Application.UseCases.Contacts.Commands.CreateEmailContact;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to create a new email contact for a person.
/// </summary>
public record CreateEmailContactCommand : BaseCommand<CreateEmailContactCommandResponse>
{
    /// <summary>
    /// Gets the person's alternate key this contact belongs to.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets the email address.
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether this is the primary email contact.
    /// </summary>
    public bool IsPrimary { get; init; }

    /// <summary>
    /// Gets additional notes or context for this contact.
    /// </summary>
    public string? Notes { get; init; }
}

/// <summary>
/// Response for the CreateEmailContactCommand.
/// </summary>
public class CreateEmailContactCommandResponse
{
    /// <summary>
    /// Gets the created contact's alternate key.
    /// </summary>
    public Guid ContactKey { get; init; }
}

