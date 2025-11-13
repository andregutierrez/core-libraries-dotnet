namespace People.Application.UseCases.Addresses.Queries.GetAddressesByPersonKey;

using Core.Libraries.Application.Queries;
using People.Application.DTOs;

/// <summary>
/// Query to get all addresses for a person.
/// </summary>
public class GetAddressesByPersonKeyQuery : IQuery<GetAddressesByPersonKeyQueryResponse>
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }
}

/// <summary>
/// Response for the GetAddressesByPersonKeyQuery.
/// </summary>
public class GetAddressesByPersonKeyQueryResponse
{
    /// <summary>
    /// Gets the collection of detailed addresses.
    /// </summary>
    public IReadOnlyList<AddressDetailDto> Addresses { get; init; } = Array.Empty<AddressDetailDto>();
}

