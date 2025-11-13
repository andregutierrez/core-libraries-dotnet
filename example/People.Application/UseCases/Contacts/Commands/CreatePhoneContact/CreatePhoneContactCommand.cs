namespace People.Application.UseCases.Contacts.Commands.CreatePhoneContact;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to create a new phone contact for a person (Phone, Mobile, or WhatsApp).
/// </summary>
public class CreatePhoneContactCommand : BaseCommand<CreatePhoneContactCommandResponse>
{
    /// <summary>
    /// Gets the person's alternate key this contact belongs to.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets the phone number.
    /// </summary>
    public string PhoneNumber { get; init; } = string.Empty;

    /// <summary>
    /// Gets the country code for the phone number (e.g., "55" for Brazil).
    /// </summary>
    public string? CountryCode { get; init; }

    /// <summary>
    /// Gets the type of phone contact (Phone = 2, Mobile = 3, WhatsApp = 4).
    /// </summary>
    public int ContactTypeId { get; init; }

    /// <summary>
    /// Gets a value indicating whether this is the primary phone contact of its type.
    /// </summary>
    public bool IsPrimary { get; init; }

    /// <summary>
    /// Gets additional notes or context for this contact.
    /// </summary>
    public string? Notes { get; init; }
}

/// <summary>
/// Response for the CreatePhoneContactCommand.
/// </summary>
public class CreatePhoneContactCommandResponse
{
    /// <summary>
    /// Gets the created contact's alternate key.
    /// </summary>
    public Guid ContactKey { get; init; }
}

