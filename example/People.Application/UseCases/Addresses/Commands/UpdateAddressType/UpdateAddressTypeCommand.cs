namespace People.Application.UseCases.Addresses.Commands;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to update the type of an address.
/// </summary>
public class UpdateAddressTypeCommand : BaseCommand
{
    /// <summary>
    /// Gets the address's alternate key.
    /// </summary>
    public Guid AddressKey { get; init; }

    /// <summary>
    /// Gets the new address type code.
    /// </summary>
    public int AddressTypeCode { get; init; }
}

