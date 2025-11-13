namespace People.Application.UseCases.Addresses.Commands.ImportAddress;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to import an address from an external system with a specific alternate key.
/// </summary>
public class ImportAddressCommand : BaseCommand<ImportAddressCommandResponse>
{
    /// <summary>
    /// Gets the alternate key from the external system.
    /// </summary>
    public Guid AddressKey { get; init; }

    /// <summary>
    /// Gets the person's alternate key this address belongs to.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets the type of address.
    /// </summary>
    public int AddressTypeCode { get; init; }

    /// <summary>
    /// Gets the street locality alternate key.
    /// </summary>
    public Guid StreetKey { get; init; }

    /// <summary>
    /// Gets the city locality alternate key.
    /// </summary>
    public Guid CityKey { get; init; }

    /// <summary>
    /// Gets the address number, if any.
    /// </summary>
    public string? Number { get; init; }

    /// <summary>
    /// Gets the address complement, if any.
    /// </summary>
    public string? Complement { get; init; }

    /// <summary>
    /// Gets the neighborhood/district locality alternate key, if any.
    /// </summary>
    public Guid? NeighborhoodKey { get; init; }

    /// <summary>
    /// Gets the state/province locality alternate key, if any.
    /// </summary>
    public Guid? StateKey { get; init; }

    /// <summary>
    /// Gets the country locality alternate key, if any.
    /// </summary>
    public Guid? CountryKey { get; init; }

    /// <summary>
    /// Gets the postal code locality alternate key, if any.
    /// </summary>
    public Guid? PostalCodeKey { get; init; }

    /// <summary>
    /// Gets a value indicating whether this is the primary address.
    /// </summary>
    public bool IsPrimary { get; init; }

    /// <summary>
    /// Gets additional notes or context for this address.
    /// </summary>
    public string? Notes { get; init; }
}

/// <summary>
/// Response for the ImportAddressCommand.
/// </summary>
public class ImportAddressCommandResponse
{
    /// <summary>
    /// Gets the imported address's alternate key.
    /// </summary>
    public Guid AddressKey { get; init; }
}

