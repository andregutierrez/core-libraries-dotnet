namespace People.Application.DTOs;

/// <summary>
/// Data transfer object representing detailed address information.
/// Used for GET queries that return complete address details.
/// </summary>
public class AddressDetailDto
{
    /// <summary>
    /// Gets the address's alternate key.
    /// </summary>
    public Guid AddressKey { get; init; }

    /// <summary>
    /// Gets the person reference this address belongs to.
    /// </summary>
    public PersonReferenceDto Person { get; init; } = null!;

    /// <summary>
    /// Gets the type of address.
    /// </summary>
    public int AddressTypeCode { get; init; }

    /// <summary>
    /// Gets the street locality reference.
    /// </summary>
    public LocalityReferenceDto Street { get; init; } = null!;

    /// <summary>
    /// Gets the address number, if any.
    /// </summary>
    public string? Number { get; init; }

    /// <summary>
    /// Gets the address complement, if any.
    /// </summary>
    public string? Complement { get; init; }

    /// <summary>
    /// Gets the neighborhood/district locality reference, if any.
    /// </summary>
    public LocalityReferenceDto? Neighborhood { get; init; }

    /// <summary>
    /// Gets the city locality reference.
    /// </summary>
    public LocalityReferenceDto City { get; init; } = null!;

    /// <summary>
    /// Gets the state/province locality reference, if any.
    /// </summary>
    public LocalityReferenceDto? State { get; init; }

    /// <summary>
    /// Gets the country locality reference, if any.
    /// </summary>
    public LocalityReferenceDto? Country { get; init; }

    /// <summary>
    /// Gets the postal code locality reference, if any.
    /// </summary>
    public LocalityReferenceDto? PostalCode { get; init; }

    /// <summary>
    /// Gets a value indicating whether this is the primary address.
    /// </summary>
    public bool IsPrimary { get; init; }

    /// <summary>
    /// Gets additional notes or context for this address.
    /// </summary>
    public string? Notes { get; init; }
}

