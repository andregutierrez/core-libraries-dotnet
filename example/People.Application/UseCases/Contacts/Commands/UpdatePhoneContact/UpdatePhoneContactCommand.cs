namespace People.Application.UseCases.Contacts.Commands.UpdatePhoneContact;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to update a phone contact's phone number.
/// </summary>
public record UpdatePhoneContactCommand : BaseCommand
{
    /// <summary>
    /// Gets the contact's alternate key.
    /// </summary>
    public Guid ContactKey { get; init; }

    /// <summary>
    /// Gets the new phone number.
    /// </summary>
    public string PhoneNumber { get; init; } = string.Empty;

    /// <summary>
    /// Gets the country code for the phone number (e.g., "55" for Brazil).
    /// </summary>
    public string? CountryCode { get; init; }
}

