namespace People.Application.UseCases.Addresses.Commands;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to set an address as the primary address for a person.
/// </summary>
public class SetAddressAsPrimaryCommand : BaseCommand
{
    /// <summary>
    /// Gets the address's alternate key.
    /// </summary>
    public Guid AddressKey { get; init; }
}

