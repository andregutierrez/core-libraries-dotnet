namespace People.Application.UseCases.Addresses.Commands;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to delete an address.
/// </summary>
public class DeleteAddressCommand : BaseCommand
{
    /// <summary>
    /// Gets the address's alternate key.
    /// </summary>
    public Guid AddressKey { get; init; }
}

