namespace People.Application.UseCases.Addresses.Queries.GetAddressByKey;

using Core.LibrariesApplication.Queries;
using People.Application.DTOs;

/// <summary>
/// Query to get an address by its alternate key.
/// </summary>
public class GetAddressByKeyQuery : IQuery<GetAddressByKeyQueryResponse?>
{
    /// <summary>
    /// Gets the address's alternate key.
    /// </summary>
    public Guid AddressKey { get; init; }
}

/// <summary>
/// Response for the GetAddressByKeyQuery.
/// </summary>
public class GetAddressByKeyQueryResponse
{
    /// <summary>
    /// Gets the detailed address information.
    /// </summary>
    public AddressDetailDto Address { get; init; } = null!;
}

