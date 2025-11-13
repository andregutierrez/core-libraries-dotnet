namespace People.Domain.Addresses.Events;

/// <summary>
/// Domain event raised when an address is created.
/// </summary>
public record AddressCreatedEvent
{
    /// <summary>
    /// Gets the address's alternate key.
    /// </summary>
    public Guid AddressKey { get; init; }

    /// <summary>
    /// Gets the person's entity ID this address belongs to.
    /// </summary>
    public int PersonId { get; init; }

    /// <summary>
    /// Gets the street address line.
    /// </summary>
    public string Street { get; init; } = string.Empty;

    /// <summary>
    /// Gets the address number, if any.
    /// </summary>
    public string? Number { get; init; }

    /// <summary>
    /// Gets the address complement, if any.
    /// </summary>
    public string? Complement { get; init; }

    /// <summary>
    /// Gets the neighborhood/district, if any.
    /// </summary>
    public string? Neighborhood { get; init; }

    /// <summary>
    /// Gets the city name.
    /// </summary>
    public string City { get; init; } = string.Empty;

    /// <summary>
    /// Gets the state/province, if any.
    /// </summary>
    public string? State { get; init; }

    /// <summary>
    /// Gets the country name, if any.
    /// </summary>
    public string? Country { get; init; }

    /// <summary>
    /// Gets the postal code, if any.
    /// </summary>
    public string? PostalCode { get; init; }

    /// <summary>
    /// Initializes a new instance of <see cref="AddressCreatedEvent"/>.
    /// </summary>
    public AddressCreatedEvent(
        Guid addressKey,
        int personId,
        string street,
        string? number,
        string? complement,
        string? neighborhood,
        string city,
        string? state,
        string? country,
        string? postalCode)
    {
        AddressKey = addressKey;
        PersonId = personId;
        Street = street;
        Number = number;
        Complement = complement;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        Country = country;
        PostalCode = postalCode;
    }
}

