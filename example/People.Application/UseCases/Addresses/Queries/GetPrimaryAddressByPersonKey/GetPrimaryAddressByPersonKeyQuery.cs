namespace People.Application.UseCases.Addresses.Queries.GetPrimaryAddressByPersonKey;

using Core.Libraries.Application.Queries;
using People.Application.DTOs;

/// <summary>
/// Query to get the primary address for a person.
/// </summary>
public class GetPrimaryAddressByPersonKeyQuery : IQuery<GetPrimaryAddressByPersonKeyQueryResponse?>
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }
}

/// <summary>
/// Response for the GetPrimaryAddressByPersonKeyQuery.
/// </summary>
public class GetPrimaryAddressByPersonKeyQueryResponse
{
    /// <summary>
    /// Gets the detailed primary address information.
    /// </summary>
    public AddressDetailDto Address { get; init; } = null!;
}

