namespace People.Application.UseCases.Localities.Queries.SearchLocalities;

using Core.Libraries.Application.Queries;
using People.Application.DTOs;

/// <summary>
/// Query to search localities with filters.
/// </summary>
public class SearchLocalitiesQuery : IQuery<SearchLocalitiesQueryResponse>
{
    /// <summary>
    /// Gets the locality name to search for (partial match). Optional.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Gets the locality type code to filter by. Optional.
    /// </summary>
    public int? LocalityTypeCode { get; init; }

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
/// Response for the SearchLocalitiesQuery.
/// </summary>
public class SearchLocalitiesQueryResponse
{
    /// <summary>
    /// Gets the collection of locality references matching the search criteria.
    /// </summary>
    public IReadOnlyList<LocalityReferenceDto> Localities { get; init; } = Array.Empty<LocalityReferenceDto>();

    /// <summary>
    /// Gets the total number of localities matching the search criteria.
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

