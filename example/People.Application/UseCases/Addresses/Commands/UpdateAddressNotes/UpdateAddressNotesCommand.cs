namespace People.Application.UseCases.Addresses.Commands;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to update the notes for an address.
/// </summary>
public class UpdateAddressNotesCommand : BaseCommand
{
    /// <summary>
    /// Gets the address's alternate key.
    /// </summary>
    public Guid AddressKey { get; init; }

    /// <summary>
    /// Gets the new notes. Can be null to remove notes.
    /// </summary>
    public string? Notes { get; init; }
}

