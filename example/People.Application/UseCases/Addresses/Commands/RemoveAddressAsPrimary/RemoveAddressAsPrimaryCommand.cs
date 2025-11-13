namespace People.Application.UseCases.Addresses.Commands.RemoveAddressAsPrimary;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to remove the primary flag from an address.
/// </summary>
public record RemoveAddressAsPrimaryCommand : BaseCommand
{
    /// <summary>
    /// Gets the address's alternate key.
    /// </summary>
    public Guid AddressKey { get; init; }
}

