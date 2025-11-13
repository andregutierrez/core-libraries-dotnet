namespace People.Application.DTOs;

/// <summary>
/// Data transfer object representing summarized address information.
/// Used for Search queries that return a list of addresses with minimal information.
/// </summary>
public class AddressSummaryDto
{
    /// <summary>
    /// Gets the address's alternate key.
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
    /// Gets a value indicating whether this is the primary address.
    /// </summary>
    public bool IsPrimary { get; init; }
}

