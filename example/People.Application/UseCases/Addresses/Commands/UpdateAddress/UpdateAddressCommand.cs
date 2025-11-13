namespace People.Application.UseCases.Addresses.Commands.UpdateAddress;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to update an existing address.
/// </summary>
public record UpdateAddressCommand : BaseCommand
{
    /// <summary>
    /// Gets the address's alternate key.
    /// </summary>
    public Guid AddressKey { get; init; }

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
}

