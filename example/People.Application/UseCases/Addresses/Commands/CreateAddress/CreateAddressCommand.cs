namespace People.Application.UseCases.Addresses.Commands.CreateAddress;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to create a new address for a person.
/// </summary>
public class CreateAddressCommand : BaseCommand<CreateAddressCommandResponse>
{
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
/// Response for the CreateAddressCommand.
/// </summary>
public class CreateAddressCommandResponse
{
    /// <summary>
    /// Gets the created address's alternate key.
    /// </summary>
    public Guid AddressKey { get; init; }
}

