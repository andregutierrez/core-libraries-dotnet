namespace People.Application.UseCases.Addresses.Queries.SearchAddresses;

using Core.Libraries.Application.Queries;
using People.Application.DTOs;

/// <summary>
/// Query to search addresses with filters.
/// </summary>
public class SearchAddressesQuery : IQuery<SearchAddressesQueryResponse>
{
    /// <summary>
    /// Gets the person's alternate key to filter by. Optional.
    /// </summary>
    public Guid? PersonKey { get; init; }

    /// <summary>
    /// Gets the address type code to filter by. Optional.
    /// </summary>
    public int? AddressTypeCode { get; init; }

    /// <summary>
    /// Gets a value indicating whether to return only primary addresses.
    /// </summary>
    public bool? IsPrimary { get; init; }

    /// <summary>
    /// Gets the city locality alternate key to filter by. Optional.
    /// </summary>
    public Guid? CityKey { get; init; }

    /// <summary>
    /// Gets the state/province locality alternate key to filter by. Optional.
    /// </summary>
    public Guid? StateKey { get; init; }

    /// <summary>
    /// Gets the country locality alternate key to filter by. Optional.
    /// </summary>
    public Guid? CountryKey { get; init; }

    /// <summary>
    /// Gets the page number for pagination. Defaults to 1.
    /// </summary>
    public int PageNumber { get; init; } = 1;

    /// <summary>
    /// Gets the page size for pagination. Defaults to 10.
    /// </summary>
    public int PageSize { get; init; } = 10;
}

/// <summary>
/// Response for the SearchAddressesQuery.
/// </summary>
public class SearchAddressesQueryResponse
{
    /// <summary>
    /// Gets the collection of summarized addresses matching the search criteria.
    /// </summary>
    public IReadOnlyList<AddressSummaryDto> Addresses { get; init; } = Array.Empty<AddressSummaryDto>();

    /// <summary>
    /// Gets the total number of addresses matching the search criteria.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Gets the current page number.
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// Gets the page size.
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

